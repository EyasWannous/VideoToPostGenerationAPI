using System.ComponentModel.DataAnnotations;

namespace VideoToPostGenerationAPI.DTOs.Incoming;

public record RequestLoginDTO
{
    [EmailAddress]
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
