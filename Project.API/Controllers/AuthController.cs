using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.API.ActionFilters;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.Core.Helper;
using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ConfigSettings _configSettings;
    private IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService, ConfigSettings configSettings)
    {
        _authService = authService;
        _userService = userService;
        _configSettings = configSettings;
    }

    [SwaggerOperation(Summary = "login")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(LoginResponseDTO))]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var userSalt = await _authService.GetUserSaltAsync(loginDTO.Username);
        if (string.IsNullOrEmpty(userSalt)) return Ok(new ErrorDataResult<Result>(Messages.InvalidModel));

        loginDTO.Password = SecurityHelper.HashPassword(loginDTO.Password, userSalt);
        IDataResult<UserToListDTO> userDTO = await _authService.LoginAsync(loginDTO);
        if (!userDTO.Success) return Ok(userDTO);

        var expirationDate = DateTime.Now.AddHours(3);

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, userDTO.Data.UserId.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, userDTO.Data.Username));
        claims.Add(new Claim(ClaimTypes.Expiration, expirationDate.ToString()));

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configSettings.AuthSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expirationDate,
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenValue = tokenHandler.WriteToken(token);

        var loginResponseDTO = new LoginResponseDTO
        {
            Token = tokenValue,
            User = userDTO.Data
        };

        return Ok(new SuccessDataResult<LoginResponseDTO>(loginResponseDTO));
    }

    [SwaggerOperation(Summary = "logout")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // implement logout logic due requirements
        return Ok();
    }
}