using Microsoft.AspNetCore.Identity;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Services.Helpers;

public class PasswordHasher : IPasswordHasher<User>
{
    private const char Delimiter = ';';

    public string HashPassword(User user, string password)
    {
        var mergedPassword = $"{user.UserName}_{password}_{user.Email}";
        var hashedPassword = HashingComplexHelper.Hash(mergedPassword, out byte[] salt);

        return string.Join(Delimiter, Convert.ToHexString(salt), hashedPassword);
    }

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
