using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.DTOs.Incoming;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Controllers;

public class AuthController(IUnitOfWork unitOfWork, IMapper mapper,
    IUserService userService, ITokenService tokenService) : BaseController(unitOfWork, mapper)
{
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;

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
                new ResponseUserRegister
                {
                    Message = result.Message,
                    IsSuccess = false,
                    Errors = result.Errors,
                }
            );

        var (JwtToken, ExpireDate) = await _tokenService.GenerateJwtTokenAsync(result.User!);

        return Ok
        (
            new ResponseUserRegister
            {
                Message = result.Message,
                IsSuccess = true,
                JwtToken = JwtToken,
                ExpireDate = ExpireDate
            }
        );
    }

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
                new ResponseUserLogin
                {
                    Message = result.Message,
                    IsSuccess = false,
                }
            );

        var (JwtToken, ExpireDate) = await _tokenService.GenerateJwtTokenAsync(result.User!);

        return Ok
        (
            new ResponseUserLogin
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
