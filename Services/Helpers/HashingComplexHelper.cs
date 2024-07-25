using System.Security.Cryptography;
using System.Text;

namespace VideoToPostGenerationAPI.Services.Helpers;

public static class HashingComplexHelper
{
    const int keySize = 512 / 8;
    const int iterations = 350000;
    readonly static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public static string Hash(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(keySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2
        (
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize
        );

        return Convert.ToHexString(hash);
    }

    public static bool Verify(string password, string hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2
        (
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize
        );

        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}