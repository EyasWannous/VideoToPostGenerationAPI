using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace VideoToPostGenerationAPI.Services.Helpers;

/// <summary>
/// Provides methods for encoding and decoding tokens using Base64 URL encoding.
/// </summary>
public static class TokenEncoderDecoder
{
    /// <summary>
    /// Encodes a token string into a Base64 URL-encoded string.
    /// </summary>
    /// <param name="token">The token string to be encoded.</param>
    /// <returns>A Base64 URL-encoded representation of the token.</returns>
    public static string Encode(string token)
    {
        var encodedToken = Encoding.UTF8.GetBytes(token);
        var validToken = Base64UrlEncoder.Encode(encodedToken);

        return validToken;
    }

    /// <summary>
    /// Decodes a Base64 URL-encoded token string back into its original form.
    /// </summary>
    /// <param name="token">The Base64 URL-encoded token string to be decoded.</param>
    /// <returns>The decoded token string in its original form.</returns>
    public static string Decode(string token)
    {
        var decodedToken = Base64UrlEncoder.DecodeBytes(token);
        var normalToken = Encoding.UTF8.GetString(decodedToken);

        return normalToken;
    }
}
