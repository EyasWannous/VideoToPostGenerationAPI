using Microsoft.AspNetCore.Identity;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Services.Helpers;

/// <summary>
/// Provides methods for hashing and verifying user passwords using a custom hashing algorithm.
/// </summary>
public class PasswordHasher : IPasswordHasher<User>
{
    private const char Delimiter = ';';

    /// <summary>
    /// Hashes the specified password using a combination of user-specific data and password.
    /// </summary>
    /// <param name="user">The user whose password is being hashed.</param>
    /// <param name="password">The raw password to be hashed.</param>
    /// <returns>A hashed password combined with a salt value.</returns>
    public string HashPassword(User user, string password)
    {
        var mergedPassword = $"{user.UserName}_{password}_{user.Email}";
        var hashedPassword = HashingComplexHelper.Hash(mergedPassword, out byte[] salt);

        return string.Join(Delimiter, Convert.ToHexString(salt), hashedPassword);
    }

    /// <summary>
    /// Verifies the provided password against the stored hashed password.
    /// </summary>
    /// <param name="user">The user whose password is being verified.</param>
    /// <param name="hashedPassword">The hashed password to compare against.</param>
    /// <param name="providedPassword">The raw password provided for verification.</param>
    /// <returns>A result indicating whether the provided password is correct.</returns>
    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        var elements = hashedPassword.Split(Delimiter);
        if (elements.Length != 2)
        {
            // Return Failed if the hashedPassword format is invalid
            return PasswordVerificationResult.Failed;
        }

        var salt = Convert.FromHexString(elements[0]);
        var hash = elements[1];

        var mergedPassword = $"{user.UserName}_{providedPassword}_{user.Email}";

        return HashingComplexHelper.Verify(mergedPassword, hash, salt)
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }
}
