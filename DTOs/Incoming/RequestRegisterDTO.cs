using System.ComponentModel.DataAnnotations;

namespace VideoToPostGenerationAPI.DTOs.Incoming;

public record RequestRegisterDTO
{
    [EmailAddress]
    [Required]
    [MinLength(5)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
