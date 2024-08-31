using System.Security.Cryptography;
using System.Text;

namespace VideoToPostGenerationAPI.Services.Helpers;

public static class HashingComplexHelper
{
    private const int KeySize = 512 / 8; // Key size in bytes
    private const int Iterations = 350000; // Number of iterations
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    public static string Hash(string password, out byte[] salt)
    {
        salt = new byte[KeySize];
        RandomNumberGenerator.Fill(salt);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            HashAlgorithm,
            KeySize
        );

        return Convert.ToHexString(hash);
    }

    public static bool Verify(string password, string hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            HashAlgorithm,
            KeySize
        );

        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}
