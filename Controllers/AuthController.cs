using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.DTOs.Incoming;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Controllers;

/// <summary>
/// Controller for handling authentication-related operations.
/// </summary>
public class AuthController(IUnitOfWork unitOfWork, IMapper mapper,
    IUserService userService, ITokenService tokenService) : BaseController(unitOfWork, mapper)
{
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The registration details.</param>
    /// <returns>A newly registered user with a JWT token.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /register
    ///     {
    ///        "email": "user@example.com",
    ///        "password": "password123",
    ///        "confirmPassword": "string"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns the newly registered user with a JWT token.</response>
    /// <response code="400">If the registration fails.</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RequestRegisterDTO request)
    {
        var result = await _userService.RegisterUserAsync(
            request.Email,
            request.Password
        );

        if (!result.IsSuccess)
            return BadRequest
            (
                new ResponseUserRegisterDTO
                {
                    Message = result.Message,
                    IsSuccess = false,
                    Errors = result.Errors,
                }
            );

        var (JwtToken, ExpireDate) = await _tokenService.GenerateJwtTokenAsync(result.User!);

        return Ok
        (
            new ResponseUserRegisterDTO
            {
                Message = result.Message,
                IsSuccess = true,
                JwtToken = JwtToken,
                ExpireDate = ExpireDate
            }
        );
    }

    /// <summary>
    /// Logs in a user using email and password.
    /// </summary>
    /// <param name="request">The login details.</param>
    /// <returns>An authenticated user with a JWT token.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /login
    ///     {
    ///        "email": "user@example.com",
    ///        "password": "password123"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns the authenticated user with a JWT token.</response>
    /// <response code="400">If the login fails.</response>
    [HttpPost("login")]
    public async Task<IActionResult> LoginEmail(RequestLoginDTO request)
    {
        var result = await _userService.LoginUserUsingEmailAsync(
            request.Email,
            request.Password
        );

        if (!result.IsSuccess)
            return BadRequest
            (
                new ResponseUserLoginDTO
                {
                    Message = result.Message,
                    IsSuccess = false,
                }
            );

        var (JwtToken, ExpireDate) = await _tokenService.GenerateJwtTokenAsync(result.User!);

        return Ok
        (
            new ResponseUserLoginDTO
            {
                Message = result.Message,
                IsSuccess = true,
                JwtToken = JwtToken,
                ExpireDate = ExpireDate
            }
        );
    }

    //[HttpGet("all")]
    //public async Task<IActionResult> GetAll()
    //{
    //    var result = await Task.FromResult(_unitOfWork.Users.GetAllAsync());

    //    return Ok(result);
    //}
}
