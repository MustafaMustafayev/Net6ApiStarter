using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.ActionFilters;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.DTO.DTOs.AuthorDTOs;
using Project.DTO.DTOs.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [SwaggerOperation(Summary = "author list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<AuthorToListDTO>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IDataResult<List<AuthorToListDTO>> authors = await _authorService.GetAsync();
        return Ok(authors);
    }

    [SwaggerOperation(Summary = "create author")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(AuthorToListDTO))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AuthorToAddDTO authorToAddDTO)
    {
        if (!ModelState.IsValid) return Ok(new ErrorDataResult<Result>(Messages.InvalidModel));

        IDataResult<Result> added = await _authorService.AddAsync(authorToAddDTO);
        return Ok(added);
    }
}