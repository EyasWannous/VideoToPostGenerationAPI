using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IUserService
{
    Task<UserManagerResponse> RegisterUserAsync(string email, string password);

    // Uncomment if you need to log in using username
    // Task<UserManagerResponse> LoginUserUsingUserNameAsync(string userName, string password);

    Task<UserManagerResponse> LoginUserUsingEmailAsync(string email, string password);

    Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);

    Task<UserManagerResponse> ForgetPasswordAsync(string email);

    Task<UserManagerResponse> ResetPasswordAsync(string email, string token, string newPassword);
}
