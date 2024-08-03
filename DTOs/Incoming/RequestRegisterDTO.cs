using System.ComponentModel.DataAnnotations;

namespace VideoToPostGenerationAPI.DTOs.Incoming;

/// <summary>
/// Represents the data transfer object for user registration requests.
/// </summary>
public record RequestRegisterDTO
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// The email address must be a valid email format, required for registration, and must be at least 5 characters long.
    /// </summary>
    [EmailAddress]
    [Required]
    [MinLength(5)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for the user.
    /// The password is required for registration and must be at least 6 characters long.
    /// </summary>
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the confirmation password for the user.
    /// The confirmation password is required for registration and must match the password, with a minimum length of 6 characters.
    /// </summary>
    [Required]
    [MinLength(6)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
