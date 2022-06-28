using AutoMapper;
using Project.BLL.Abstract;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.DTOs.CustomLoggingDTOs;
using Project.Entity.Entities;

namespace Project.BLL.Concrete;

public class LoggingService : ILoggingService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public LoggingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddLogAsync(RequestLogDTO requestLogDTO)
    {
        var requestLog = _mapper.Map<RequestLog>(requestLogDTO);
        await _unitOfWork.LoggingRepository.AddLogAsync(requestLog);
        await _unitOfWork.CommitAsync();
    }
}