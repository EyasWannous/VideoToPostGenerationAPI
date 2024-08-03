namespace VideoToPostGenerationAPI.Domain.Settings;

/// <summary>
/// Contains settings related to JSON Web Token (JWT) configuration.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// The key used to sign the JWT tokens.
    /// </summary>
    /// <remarks>
    /// This key should be kept secret and secure to ensure the integrity of the JWT tokens.
    /// It is used in conjunction with the signing algorithm to create a signature for the token.
    /// </remarks>
    public string SigningKey { get; set; } = string.Empty;

    /// <summary>
    /// The amount of time the JWT token is valid before it expires.
    /// </summary>
    /// <remarks>
    /// This value is specified as a <see cref="TimeSpan"/> and determines the duration for which the token remains valid.
    /// It should be configured based on the security requirements and user session duration.
    /// </remarks>
    public TimeSpan ExpireTime { get; set; }

    /// <summary>
    /// The issuer of the JWT token.
    /// </summary>
    /// <remarks>
    /// This value represents the entity that issues the token, such as the application or service responsible for authentication.
    /// It is used to validate that the token was issued by a trusted source.
    /// </remarks>
    public string ValidIssuer { get; set; } = string.Empty;

    /// <summary>
    /// The intended audience for the JWT token.
    /// </summary>
    /// <remarks>
    /// This value specifies the recipients of the token. It is used to ensure that the token is being used by the intended audience.
    /// This helps in preventing misuse of the token by unauthorized parties.
    /// </remarks>
    public string ValidAudience { get; set; } = string.Empty;
}
