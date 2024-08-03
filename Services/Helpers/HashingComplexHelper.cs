using System.Security.Cryptography;
using System.Text;

namespace VideoToPostGenerationAPI.Services.Helpers;

/// <summary>
/// Helper class for complex hashing and verification using PBKDF2 with SHA-512.
/// </summary>
public static class HashingComplexHelper
{
    private const int KeySize = 512 / 8; // Key size in bytes
    private const int Iterations = 350000; // Number of iterations
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    /// <summary>
    /// Hashes a password using PBKDF2 with SHA-512 and generates a salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <param name="salt">The generated salt used for hashing.</param>
    /// <returns>The hashed password as a hexadecimal string.</returns>
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

    /// <summary>
    /// Verifies a password against a hashed value using the provided salt.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="hash">The hashed password to compare against.</param>
    /// <param name="salt">The salt used to hash the password.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
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
