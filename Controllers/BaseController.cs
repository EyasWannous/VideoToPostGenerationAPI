using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoToPostGenerationAPI.Domain.Abstractions;

namespace VideoToPostGenerationAPI.Controllers;

/// <summary>
/// Base controller providing common functionalities for derived controllers.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseController"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for managing database operations.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public BaseController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// The unit of work for managing database operations.
    /// </summary>
    protected readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// The AutoMapper instance for object mapping.
    /// </summary>
    protected readonly IMapper _mapper;
}
