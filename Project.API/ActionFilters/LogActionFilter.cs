﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Project.BLL.Abstract;
using Project.Core.Abstract;
using Project.Core.Helper;
using Project.DTO.DTOs.CustomLoggingDTOs;

namespace Project.API.ActionFilters;

public class LogActionFilter : IAsyncActionFilter
{
    private readonly ConfigSettings _configSettings;
    private readonly ILoggingService _loggingService;
    private readonly IUtilService _utilService;

    public LogActionFilter(IUtilService utilService, ILoggingService loggingService, ConfigSettings configSettings)
    {
        _utilService = utilService;
        _loggingService = loggingService;
        _configSettings = configSettings;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;

        var traceIdentitier = httpContext?.TraceIdentifier;
        var clientIP = httpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
        var uri = httpContext.Request.Host + httpContext.Request.Path;

        var token = string.Empty;
        int? userId = null;
        var authHeaderName = _configSettings.AuthSettings.HeaderName;

        if (!string.IsNullOrEmpty(httpContext.Request.Headers[authHeaderName]) &&
            httpContext.Request.Headers[authHeaderName].ToString().Length > 7)
        {
            token = httpContext.Request.Headers[authHeaderName].ToString();
            userId = !string.IsNullOrEmpty(token)
                ? _utilService.GetUserIdFromToken(httpContext.Request.Headers[authHeaderName].ToString())
                : null;
        }

        context.HttpContext.Request.Body.Position = 0;
        using var streamReader = new StreamReader(context.HttpContext.Request.Body);
        var bodyContent = await streamReader.ReadToEndAsync();
        context.HttpContext.Request.Body.Position = 0;

        var requestLog = new RequestLogDTO
        {
            TraceIdentifier = traceIdentitier,
            ClientIP = clientIP,
            URI = uri,
            Payload = bodyContent,
            Method = httpContext.Request.Method,
            Token = token,
            UserId = userId,
            RequestDate = DateTime.Now
        };

        await next();

        var responseStatusCode = httpContext.Response.StatusCode;
        var responseLog = new ResponseLogDTO
        {
            TraceIdentifier = traceIdentitier,
            ResponseDate = DateTime.Now,
            StatusCode = responseStatusCode.ToString(),
            Token = token,
            UserId = userId
        };

        requestLog.ResponseLog = responseLog;
        await _loggingService.AddLogAsync(requestLog);
    }
}