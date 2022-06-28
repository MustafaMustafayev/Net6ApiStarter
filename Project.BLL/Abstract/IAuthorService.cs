using Project.DTO.DTOs.AuthorDTOs;
using Project.DTO.DTOs.Responses;

namespace Project.BLL.Abstract;

public interface IAuthorService
{
    Task<IDataResult<Result>> AddAsync(AuthorToAddDTO authorToAddDTO);

    Task<IDataResult<List<AuthorToListDTO>>> GetAsync();
}