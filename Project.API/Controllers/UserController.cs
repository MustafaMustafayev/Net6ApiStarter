using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.ActionFilters;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.Core.Helper;
using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;
using Sentry;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
public class UserController : Controller
{
    private readonly ConfigSettings _configSettings;
    private readonly IUserService _userService;

    public UserController(IUserService userService, ConfigSettings configSettings)
    {
        _userService = userService;
        _configSettings = configSettings;
    }

    [SwaggerOperation(Summary = "get users")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<UserToListDTO>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var pageIndex = Convert.ToInt32(HttpContext.Request.Headers[_configSettings.RequestSettings.PageIndex]);
        var pageSize = Convert.ToInt32(HttpContext.Request.Headers[_configSettings.RequestSettings.PageSize]);
        return Ok(await _userService.GetAsync(pageIndex, pageSize));
    }

    [SwaggerOperation(Summary = "get specific user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(UserToListDTO))]
    [HttpGet("{userId}")]
    public async Task<IActionResult> Get([FromRoute] int userId)
    {
        return Ok(await _userService.GetAsync(userId));
    }

    [AllowAnonymous]
    [SwaggerOperation(Summary = "create user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(UserToListDTO))]
    [HttpPost("register")]
    public async Task<IActionResult> Add([FromBody] UserToAddDTO userToAddDTO)
    {
        if (!ModelState.IsValid) return Ok(new ErrorDataResult<Result>(Messages.InvalidModel));

        return Ok(await _userService.AddAsync(userToAddDTO));
    }

    [SwaggerOperation(Summary = "update user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(UserToListDTO))]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserToUpdateDTO userToUpdateDTO)
    {
        if (!ModelState.IsValid) return Ok(new ErrorDataResult<Result>(Messages.InvalidModel));

        return Ok(await _userService.UpdateAsync(userToUpdateDTO));
    }

    [SwaggerOperation(Summary = "delete user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete([FromRoute] int userId)
    {
        await _userService.DeleteAsync(userId);
        return Ok(new SuccessDataResult<Result>());
    }

    [SwaggerOperation(Summary = "reset password")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
    [HttpPatch("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
    {
        if (!ModelState.IsValid) return Ok(new ErrorDataResult<Result>(Messages.InvalidModel));

        await _userService.ResetPasswordAsync(resetPasswordDTO);
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("test")]
    public IActionResult Test()
    {
        SentrySdk.CaptureMessage("Hello Sentry");
        throw new Exception("olMAZ");
        return Ok(new SuccessDataResult<Result>(Messages.InvalidModel));
    }
}