using System.Security.Cryptography;
using System.Text;

namespace VideoToPostGenerationAPI.Services.Helpers;

public static class HashingSimpleHelper
{
    private const int KeySize = 256 / 8; // Key size in bytes
    private const int Iterations = 1000; // Number of iterations
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA1;
    private const char Delimiter = ';';

    public static string Hash(string rawData)
    {
        var hashedPassword = Hash(rawData, out byte[] salt);
        return string.Join(Delimiter, Convert.ToHexString(salt), hashedPassword);
    }

    public static bool VerifyHashed(string hashedRawData, string rawData)
    {
        var elements = hashedRawData.Split(Delimiter);
        var salt = Convert.FromHexString(elements[0]);
        var hash = elements[1];

        return Verify(rawData, hash, salt);
    }

    private static string Hash(string password, out byte[] salt)
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

    private static bool Verify(string password, string hash, byte[] salt)
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
