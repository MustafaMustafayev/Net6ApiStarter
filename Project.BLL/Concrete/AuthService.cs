using AutoMapper;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;

namespace Project.BLL.Concrete;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<string> GetUserSaltAsync(string userName)
    {
        return await _unitOfWork.UserRepository.GetUserSaltAsync(userName);
    }

    public async Task<IDataResult<UserToListDTO>> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(m =>
            m.Username == loginDTO.Username && m.Password == loginDTO.Password);
        if (user == null) return new ErrorDataResult<UserToListDTO>(Messages.InvalidUserCredentials);

        return new SuccessDataResult<UserToListDTO>(_mapper.Map<UserToListDTO>(user));
    }
}