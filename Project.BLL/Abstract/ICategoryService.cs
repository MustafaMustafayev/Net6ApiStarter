using Project.DTO.DTOs.CategoryDTOs;
using Project.DTO.DTOs.Responses;

namespace Project.BLL.Abstract;

public interface ICategoryService
{
    Task<IDataResult<List<CategoryToListDTO>>> GetAsync();
}