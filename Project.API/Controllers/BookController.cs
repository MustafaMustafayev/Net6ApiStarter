using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.ActionFilters;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.DTO.DTOs.BookDTOs;
using Project.DTO.DTOs.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
public class BookController : Controller
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [SwaggerOperation(Summary = "book list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<BookToListDTO>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IDataResult<List<BookToListDTO>> books = await _bookService.GetAsync();
        return Ok(books);
    }

    [SwaggerOperation(Summary = "create book")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(BookToListDTO))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] BookToAddDTO bookToAddDTO)
    {
        if (!ModelState.IsValid) return Ok(new ErrorDataResult<Result>(Messages.InvalidModel));

        IDataResult<Result> added = await _bookService.AddAsync(bookToAddDTO);
        return Ok(added);
    }

    [SwaggerOperation(Summary = "search books by filter")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<BookToListDTO>))]
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] Dictionary<string, string> filters)
    {
        IDataResult<List<BookToListDTO>> books = await _bookService.Search(filters);
        return Ok(books);
    }
}