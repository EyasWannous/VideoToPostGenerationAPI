namespace VideoToPostGenerationAPI.DTOs.Outgoing;

/// <summary>
/// Represents the data transfer object for user registration responses.
/// </summary>
public record ResponseUserRegisterDTO
{
    /// <summary>
    /// Gets or sets the message associated with the registration response.
    /// This message can provide additional information about the registration result.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the registration was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets a collection of error messages related to the registration attempt.
    /// This property is useful for providing detailed feedback on what went wrong during registration.
    /// </summary>
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// Gets or sets the expiration date and time of the JWT token, if applicable.
    /// This property may be null if no token is provided or if the registration was not successful.
    /// </summary>
    public DateTime? ExpireDate { get; set; }

    /// <summary>
    /// Gets or sets the JSON Web Token (JWT) for the user, if applicable.
    /// This property may be null if the registration was not successful or no token is issued.
    /// </summary>
    public string? JwtToken { get; set; }
}
