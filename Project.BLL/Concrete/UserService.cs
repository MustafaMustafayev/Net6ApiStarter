using AutoMapper;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.Core.Helper;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DAL.Utility;
using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;
using Project.Entity.Entities;

namespace Project.BLL.Concrete;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<Result>> AddAsync(UserToAddDTO userToAddDTO)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(userToAddDTO.Username, null))
            return new ErrorDataResult<Result>(Messages.UserIsExist);

        var user = _mapper.Map<User>(userToAddDTO);
        user.Salt = SecurityHelper.GenerateSalt();
        user.Password = SecurityHelper.HashPassword(user.Password, user.Salt);
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();
        return new SuccessDataResult<Result>(null, Messages.Success);
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(m => m.UserId == userId);
        user.IsDeleted = true;
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IDataResult<PaginatedList<UserToListDTO>>> GetAsync(int pageIndex, int pageSize)
    {
        var users = _unitOfWork.UserRepository.GetAsNoTrackingList();
        var response = await PaginatedList<User>.CreateAsync(users.OrderBy(m => m.UserId), pageIndex, pageSize);
        var responseDTO = new PaginatedList<UserToListDTO>(_mapper.Map<List<UserToListDTO>>(response.Datas),
            response.TotalRecordCount, response.PageIndex, response.TotalPageCount);
        return new SuccessDataResult<PaginatedList<UserToListDTO>>(responseDTO);
    }

    public async Task<IDataResult<UserToListDTO>> GetAsync(int userId)
    {
        var user = _mapper.Map<UserToListDTO>(
            await _unitOfWork.UserRepository.GetAsNoTrackingAsync(m => m.UserId == userId));
        return new SuccessDataResult<UserToListDTO>(user);
    }

    public async Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(m => m.UserId == resetPasswordDTO.UserId);
        user.Salt = SecurityHelper.GenerateSalt();
        user.Password = SecurityHelper.HashPassword(resetPasswordDTO.Password, user.Salt);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IDataResult<Result>> UpdateAsync(UserToUpdateDTO userToUpdateDTO)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(userToUpdateDTO.Username, userToUpdateDTO.UserId))
            return new ErrorDataResult<Result>(Messages.UserIsExist);

        var user = _mapper.Map<User>(userToUpdateDTO);

        await _unitOfWork.UserRepository.UpdateUserAsync(user);

        await _unitOfWork.CommitAsync();
        return new SuccessDataResult<Result>(Messages.Success);
    }
}