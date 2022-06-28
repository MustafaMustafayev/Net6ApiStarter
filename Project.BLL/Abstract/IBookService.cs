using Project.DTO.DTOs.BookDTOs;
using Project.DTO.DTOs.Responses;

namespace Project.BLL.Abstract;

public interface IBookService
{
    Task<IDataResult<Result>> AddAsync(BookToAddDTO bookToAddDTO);

    Task<IDataResult<List<BookToListDTO>>> GetAsync();

    Task<IDataResult<List<BookToListDTO>>> Search(Dictionary<string, string> filters);
}