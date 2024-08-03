using Microsoft.AspNetCore.Identity;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.DTOs.Outgoing;
using VideoToPostGenerationAPI.Services.Helpers;

namespace VideoToPostGenerationAPI.Services;

/// <summary>
/// Service for managing user-related operations.
/// </summary>
public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _mailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userManager">The UserManager for handling user operations.</param>
    /// <param name="configuration">Configuration settings for the application.</param>
    /// <param name="mailService">The service for sending emails.</param>
    public UserService(UserManager<User> userManager, IConfiguration configuration,
        IEmailService mailService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _mailService = mailService;
    }

    /// <summary>
    /// Confirms the email for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="token">The email confirmation token.</param>
    /// <returns>A response indicating the result of the email confirmation.</returns>
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
                Message = "Email confirmation failed.",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description).ToArray()
            };

        return new UserManagerResponse { Message = "Email confirmed successfully!", IsSuccess = true };
    }

    /// <summary>
    /// Sends a password reset email to a user.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <returns>A response indicating the result of the password reset email request.</returns>
    public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return new UserManagerResponse { Message = "No user associated with this email.", IsSuccess = false };

        _ = Task.Run(() => SendResetPasswordEmail(user));

        return new UserManagerResponse { Message = "Reset password URL has been sent to the email successfully!", IsSuccess = true };
    }

    /// <summary>
    /// Logs in a user using their email and password.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A response indicating the result of the login attempt.</returns>
    public async Task<UserManagerResponse> LoginUserUsingEmailAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return new UserManagerResponse { Message = "No user found with this email.", IsSuccess = false, User = null };

        var result = await _userManager.CheckPasswordAsync(user, password);
        if (!result)
            return new UserManagerResponse { Message = "Incorrect email or password.", IsSuccess = false, User = null };

        return new UserManagerResponse { Message = "Logged in successfully!", IsSuccess = true, User = user };
    }

    // Uncomment if you need to log in using username
    // /// <summary>
    // /// Logs in a user using their username and password.
    // /// </summary>
    // /// <param name="userName">The username of the user.</param>
    // /// <param name="password">The password of the user.</param>
    // /// <returns>A response indicating the result of the login attempt.</returns>
    // public async Task<UserManagerResponse> LoginUserUsingUserNameAsync(string userName, string password)
    // {
    //     var user = await _userManager.FindByNameAsync(userName);
    //     if (user is null)
    //         return new UserManagerResponse { Message = "No user found with this username.", IsSuccess = false };

    //     var result = await _userManager.CheckPasswordAsync(user, password);
    //     if (!result)
    //         return new UserManagerResponse { Message = "Incorrect username or password.", IsSuccess = false };

    //     return new UserManagerResponse { Message = "Logged in successfully!", IsSuccess = true, User = user };
    // }

    /// <summary>
    /// Registers a new user with the specified email and password.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A response indicating the result of the registration attempt.</returns>
    public async Task<UserManagerResponse> RegisterUserAsync(string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
            return new UserManagerResponse
            {
                Message = "Email is already taken. Email must be unique.",
                IsSuccess = false,
            };

        var user = new User
        {
            Email = email,
            UserName = email
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return new UserManagerResponse
            {
                Message = "User creation failed.",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description).ToArray(),
            };

        _ = Task.Run(() => SendConfirmRequest(user));

        return new UserManagerResponse { Message = "User created successfully!", IsSuccess = true, User = user };
    }

    /// <summary>
    /// Resets the password for a user.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="token">The password reset token.</param>
    /// <param name="newPassword">The new password for the user.</param>
    /// <returns>A response indicating the result of the password reset attempt.</returns>
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
                Message = "Password reset failed.",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description).ToArray()
            };

        return new UserManagerResponse { Message = "Password has been reset successfully!", IsSuccess = true };
    }

    /// <summary>
    /// Sends an email confirmation request to the user.
    /// </summary>
    /// <param name="user">The user to send the confirmation request to.</param>
    private async Task SendConfirmRequest(User user)
    {
        var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var validEmailToken = TokenEncoderDecoder.Encode(confirmEmailToken);

        string url = $"{_configuration["AppUrl"]}/api/auth/confirmemail?userId={user.Id}&token={validEmailToken}";

        await _mailService.SendEmailAsync
        (
            user.Email!,
            "Confirm your email",
            "<h1>Welcome to BudgetBlitz</h1>"
            + "<p>Please confirm your email by "
            + $"<a href='{url}'>"
            + "clicking here</a></p>"
        );
    }

    /// <summary>
    /// Sends a password reset email to the user.
    /// </summary>
    /// <param name="user">The user to send the password reset email to.</param>
    private async Task SendResetPasswordEmail(User user)
    {
        var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var validResetToken = TokenEncoderDecoder.Encode(passwordResetToken);

        string url = $"{_configuration["AppUrl"]}/api/auth/resetpassword?email={user.Email}&token={validResetToken}";

        await _mailService.SendEmailAsync
        (
            user.Email!,
            "Reset Password",
            "<h1>Follow the instructions to reset your password</h1>"
            + "<p>To reset your password, "
            + $"<a href='{url}'>"
            + "click here</a></p>"
        );
    }
}
