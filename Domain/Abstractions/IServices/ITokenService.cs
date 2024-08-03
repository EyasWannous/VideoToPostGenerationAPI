using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

/// <summary>
/// Defines methods for handling JWT token operations.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the JWT token is to be generated.</param>
    /// <returns>A task representing the asynchronous operation, containing a tuple with:
    /// - <see cref="string"/>: The generated JWT token.
    /// - <see cref="DateTime"/>: The expiration date of the token.</returns>
    Task<(string JwtToken, DateTime ExpireDate)> GenerateJwtTokenAsync(User user);

    /// <summary>
    /// Verifies the validity of the specified JWT token.
    /// </summary>
    /// <param name="jwtToken">The JWT token to verify.</param>
    /// <returns>A task representing the asynchronous operation, containing a tuple with:
    /// - <see cref="string"/>: A message indicating the result of the verification.
    /// - <see cref="bool"/>: A boolean indicating whether the token is valid.</returns>
    Task<(string Message, bool IsSuccess)> VerfiyTokenAsync(string jwtToken);
}
