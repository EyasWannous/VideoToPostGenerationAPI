using Microsoft.AspNetCore.Identity;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.DTOs.Outgoing;
using VideoToPostGenerationAPI.Services.Helpers;

namespace VideoToPostGenerationAPI.Services;

public class UserService(UserManager<User> userManager, IConfiguration configuration, IEmailService mailService) : IUserService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly IEmailService _mailService = mailService;


    public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return new UserManagerResponse { Message = "User not found.", IsSuccess = false };

        var normalToken = TokenEncoderDecoder.Decode(token);

        var result = await _userManager.ConfirmEmailAsync(user, normalToken);
        if (!result.Succeeded)
            return new UserManagerResponse
            {
                Message = "Email didn't confirmed.",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description).ToArray()
            };

        return new UserManagerResponse { Message = "Email Confirmed Successfully!", IsSuccess = true };
    }


    public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return new UserManagerResponse { Message = "No user associated with this Email.", IsSuccess = false };

        _ = Task.Run(() => SendResetPasswordEmail(user));

        return new UserManagerResponse { Message = "Reset Password URL has been sent to the email Successfully!", IsSuccess = true };
    }


    public async Task<UserManagerResponse> LoginUserUsingEmailAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return new UserManagerResponse { Message = "There is No user with that Email.", IsSuccess = false, User = null };

        var result = await _userManager.CheckPasswordAsync(user, password);
        if (!result)
            return new UserManagerResponse { Message = "Email or Password is not correct.", IsSuccess = false, User = null };

        return new UserManagerResponse { Message = "Logged in Successfully!", IsSuccess = true, User = user };
    }


    //public async Task<UserManagerResponse> LoginUserUsingUserNameAsync(string userName, string password)
    //{
    //    var user = await _userManager.FindByNameAsync(userName);
    //    if (user is null)
    //        return new UserManagerResponse { Message = "There is a user with that UserName.", IsSuccess = false };

    //    var result = await _userManager.CheckPasswordAsync(user, password);
    //    if (!result)
    //        return new UserManagerResponse { Message = "UserName or Password is not correct.", IsSuccess = false };

    //    return new UserManagerResponse { Message = "Logged in Successfully!", IsSuccess = true, User = user };
    //}


    public async Task<UserManagerResponse> RegisterUserAsync(string email, string password)
    {
        var checkEmail = await _userManager.FindByEmailAsync(email);
        if (checkEmail is not null)
            return new UserManagerResponse
            {
                Message = "Email is already taken, Email must be unique.",
                IsSuccess = false,
            };

        var user = new User
        {
            Email = email,
            UserName = email
        };

        // -----------------------------------------------------------------
        // Don't do this=
        // var hashedPassword = _passwordHasher.HashPassword(user, password);
        // var result = await _userManager.CreateAsync(user, hashedPassword);
        // -----------------------------------------------------------------

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return new UserManagerResponse
            {
                Message = "User dosen't created",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description).ToArray(),
            };

        _ = Task.Run(() => SendConfirmRequest(user));

        return new UserManagerResponse { Message = "User created Successfully!", IsSuccess = true, User = user };
    }


    public async Task<UserManagerResponse> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return new UserManagerResponse { Message = "No user associated with this email.", IsSuccess = false };

        var normalToken = TokenEncoderDecoder.Decode(token);

        var result = await _userManager.ResetPasswordAsync(user, normalToken, newPassword);
        if (!result.Succeeded)
            return new UserManagerResponse
            {
                Message = "Something went wrong.",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description).ToArray()
            };

        return new UserManagerResponse { Message = "Password has been reset Successfully!", IsSuccess = true };
    }


    private async Task SendConfirmRequest(User user)
    {
        var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var validEmailToken = TokenEncoderDecoder.Encode(confirmEmailToken);

        string url = $"{_configuration["AppUrl"]}/api/auth/confirmemail?userId={user.Id}&token={validEmailToken}";

        await _mailService.SendEmailAsync
        (
            user.Email!,
            "Confirm your email",
            "<h1>Welcom to BudgetBlitz</h1>"
            + "<p>Please confirm your email by "
            + $"<a href='{url}'>"
            + "Clicking here</a></p>"
        );
    }


    private async Task SendResetPasswordEmail(User user)
    {
        var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var validResetToken = TokenEncoderDecoder.Encode(passwordResetToken);

        string url = $"{_configuration["AppUrl"]}/api/auth/resetpassword?email={user.Email}&token={validResetToken}";

        await _mailService.SendEmailAsync
        (
            user.Email!,
            "Reset Password",
            "<h1>Follow the instructions to reset the password</h1>"
            + "<p>To reset your password "
            + $"<a href='{url}'>"
            + "Clicking here</a></p>"
        );
    }
}