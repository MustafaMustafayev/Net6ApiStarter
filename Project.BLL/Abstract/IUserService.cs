using Project.DAL.Utility;
using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;

namespace Project.BLL.Abstract;

public interface IUserService
{
    Task<IDataResult<PaginatedList<UserToListDTO>>> GetAsync(int pageIndex, int pageSize);

    Task<IDataResult<UserToListDTO>> GetAsync(int userId);

    Task<IDataResult<Result>> AddAsync(UserToAddDTO userToAddDTO);

    Task<IDataResult<Result>> UpdateAsync(UserToUpdateDTO userToUpdateDTO);

    Task DeleteAsync(int userId);

    Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
}