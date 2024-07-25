using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoToPostGenerationAPI.Domain.Abstractions;

namespace VideoToPostGenerationAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly IMapper _mapper = mapper;
}
