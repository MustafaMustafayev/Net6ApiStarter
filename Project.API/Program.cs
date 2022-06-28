﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Project.API.ActionFilters;
using Project.BLL.Abstract;
using Project.BLL.Concrete;
using Project.BLL.Mappers;
using Project.Core.Abstract;
using Project.Core.Concrete;
using Project.Core.CustomMiddlewares.ExceptionHandler;
using Project.Core.CustomMiddlewares.Translation;
using Project.Core.Helper;
using Project.Core.Logging;
using Project.DAL.Abstract;
using Project.DAL.Concrete;
using Project.DAL.DatabaseContext;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DAL.UnitOfWorks.Concrete;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var configurationManager = builder.Configuration;

var configSettings = new ConfigSettings();

configurationManager.GetSection("Config").Bind(configSettings);

builder.Services.AddSingleton(configSettings);

builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

builder.Services.AddControllers();

builder.WebHost.UseSentry();

builder.Services.AddAutoMapper(typeof(Automapper));

builder.Services.AddDbContext<DataContext>(options => { options.UseNpgsql(configSettings.ConnectionStrings.AppDb); });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ILoggingRepository, LoggingRepository>();

builder.Services.AddScoped<ILoggingService, LoggingService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IUtilService, UtilService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

/*
builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    options.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;
});
*/

builder.Services.AddHealthChecks();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configSettings.AuthSettings.SecretKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.LogoutPath = "/auth/logout";
    options.SlidingExpiration = true;

    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = ".AspNetCore.Security.Cookie",
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
});

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b =>
{
    b
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins("http://localhost:4200");
}));

builder.Services.AddScoped<LogActionFilter>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "REST API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
// }
app.UseSwagger();
app.UseSwaggerUI();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseCors("CorsPolicy");

app.UseMiddleware<LocalizationMiddleware>();
app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

// app.UseMiniProfiler();
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Frame-Options", "Deny");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    await next.Invoke();
});

app.UseSentryTracing();

app.UseStaticFiles();

app.UseAuthorization();

app.UseAuthentication();

app.UseHealthChecks("/hc");

app.MapControllers();

app.Run();