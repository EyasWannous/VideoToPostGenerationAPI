using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace VideoToPostGenerationAPI.Services.Helpers;

public static class HashingSimpleHelper
{
    const int keySize = 256 / 8;
    const int iterations = 1000;
    readonly static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA1;
    const char Delimiter = ';';

    public static string Hash(string rawData)
    {
        var hasedPassword = Hash(rawData, out byte[] salt);

        return string.Join(Delimiter, Convert.ToHexString(salt), hasedPassword);
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

    private static bool Verify(string password, string hash, byte[] salt)
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
