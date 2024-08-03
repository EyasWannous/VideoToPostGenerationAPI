using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

/// <summary>
/// Represents the data transfer object for responses from user management operations.
/// </summary>
public record UserManagerResponse
{
    /// <summary>
    /// Gets or sets the user entity associated with the response, if applicable.
    /// This property may be null if the user management operation was not successful or if no user is returned.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets the message associated with the response.
    /// This message provides additional information about the result of the user management operation.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user management operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets a collection of error messages related to the user management operation.
    /// This property provides detailed feedback on any issues encountered during the operation.
    /// </summary>
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// Gets or sets the expiration date and time related to the response, if applicable.
    /// This property may be null if no expiration date is relevant or if the operation was not successful.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }
}
