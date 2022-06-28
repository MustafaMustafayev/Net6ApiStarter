using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.ActionFilters;
using Project.BLL.Abstract;
using Project.DTO.DTOs.CategoryDTOs;
using Project.DTO.DTOs.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [SwaggerOperation(Summary = "category list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<CategoryToListDTO>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IDataResult<List<CategoryToListDTO>> categories = await _categoryService.GetAsync();
        return Ok(categories);
    }
}