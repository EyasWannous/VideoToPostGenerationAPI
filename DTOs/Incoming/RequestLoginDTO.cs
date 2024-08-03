using System.ComponentModel.DataAnnotations;

namespace VideoToPostGenerationAPI.DTOs.Incoming;

/// <summary>
/// Represents the data transfer object for user login requests.
/// </summary>
public record RequestLoginDTO
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// The email address must be a valid email format and is required for login.
    /// </summary>
    [EmailAddress]
    [Required]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for the user.
    /// The password is required for login.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}
