namespace VideoToPostGenerationAPI.DTOs.Outgoing;

/// <summary>
/// Represents the data transfer object for user login responses.
/// </summary>
public record ResponseUserLoginDTO
{
    /// <summary>
    /// Gets or sets the message associated with the login response.
    /// This message can provide additional information about the login result.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the login was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the expiration date and time of the JWT token, if applicable.
    /// This property may be null if no token is provided or if the login was not successful.
    /// </summary>
    public DateTime? ExpireDate { get; set; }

    /// <summary>
    /// Gets or sets the JSON Web Token (JWT) for the user, if applicable.
    /// This property may be null if the login was not successful or no token is issued.
    /// </summary>
    public string? JwtToken { get; set; }
}
