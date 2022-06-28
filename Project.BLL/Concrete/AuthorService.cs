using AutoMapper;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.DTOs.AuthorDTOs;
using Project.DTO.DTOs.Responses;
using Project.Entity.Entities;

namespace Project.BLL.Concrete;

public class AuthorService : IAuthorService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<Result>> AddAsync(AuthorToAddDTO authorToAddDTO)
    {
        var author = _mapper.Map<Author>(authorToAddDTO);
        await _unitOfWork.AuthorRepository.AddAsync(author);
        await _unitOfWork.CommitAsync();

        return new SuccessDataResult<Result>(Messages.Success);
    }

    public async Task<IDataResult<List<AuthorToListDTO>>> GetAsync()
    {
        var authors = await _unitOfWork.AuthorRepository.GetListAsync();
        var authorToListDTOs = _mapper.Map<List<AuthorToListDTO>>(authors);
        return new SuccessDataResult<List<AuthorToListDTO>>(authorToListDTOs, Messages.Success);
    }
}