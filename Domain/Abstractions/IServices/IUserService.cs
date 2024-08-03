using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

/// <summary>
/// Defines operations related to user management.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Registers a new user with the specified email and password.
    /// </summary>
    /// <param name="email">The email address of the user to register.</param>
    /// <param name="password">The password for the new user account.</param>
    /// <returns>A <see cref="UserManagerResponse"/> indicating the result of the registration attempt.</returns>
    Task<UserManagerResponse> RegisterUserAsync(string email, string password);

    // Uncomment if you need to log in using username
    // /// <summary>
    // /// Logs in a user using their username and password.
    // /// </summary>
    // /// <param name="userName">The username of the user.</param>
    // /// <param name="password">The password of the user.</param>
    // /// <returns>A <see cref="UserManagerResponse"/> indicating the result of the login attempt.</returns>
    // Task<UserManagerResponse> LoginUserUsingUserNameAsync(string userName, string password);

    /// <summary>
    /// Logs in a user using their email and password.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A <see cref="UserManagerResponse"/> indicating the result of the login attempt.</returns>
    Task<UserManagerResponse> LoginUserUsingEmailAsync(string email, string password);

    /// <summary>
    /// Confirms the email address for a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="token">The token used to confirm the email address.</param>
    /// <returns>A <see cref="UserManagerResponse"/> indicating the result of the email confirmation attempt.</returns>
    Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);

    /// <summary>
    /// Sends a password reset email to a user.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>A <see cref="UserManagerResponse"/> indicating the result of the password reset email request.</returns>
    Task<UserManagerResponse> ForgetPasswordAsync(string email);

    /// <summary>
    /// Resets the password for a user.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="token">The token used to reset the password.</param>
    /// <param name="newPassword">The new password for the user.</param>
    /// <returns>A <see cref="UserManagerResponse"/> indicating the result of the password reset attempt.</returns>
    Task<UserManagerResponse> ResetPasswordAsync(string email, string token, string newPassword);
}
