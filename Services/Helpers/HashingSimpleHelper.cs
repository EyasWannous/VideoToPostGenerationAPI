using System.Security.Cryptography;
using System.Text;

namespace VideoToPostGenerationAPI.Services.Helpers;

/// <summary>
/// Helper class for simple hashing and verification using PBKDF2 with SHA-1.
/// </summary>
public static class HashingSimpleHelper
{
    private const int KeySize = 256 / 8; // Key size in bytes
    private const int Iterations = 1000; // Number of iterations
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA1;
    private const char Delimiter = ';';

    /// <summary>
    /// Hashes raw data using PBKDF2 with SHA-1 and includes salt in the result.
    /// </summary>
    /// <param name="rawData">The data to hash.</param>
    /// <returns>A string containing the salt and hashed data, separated by a delimiter.</returns>
    public static string Hash(string rawData)
    {
        var hashedPassword = Hash(rawData, out byte[] salt);
        return string.Join(Delimiter, Convert.ToHexString(salt), hashedPassword);
    }

    /// <summary>
    /// Verifies if the hashed data matches the raw data provided.
    /// </summary>
    /// <param name="hashedRawData">The hashed data containing salt and hash.</param>
    /// <param name="rawData">The raw data to verify.</param>
    /// <returns>True if the raw data matches the hashed data; otherwise, false.</returns>
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
