using System.ComponentModel.DataAnnotations;
using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.DTOs.Incoming;

public record RequestPostDTO
{
    [Required]
    public required string Platform { get; set; }
    public PointOfView PointOfView { get; set; } = 0;
    //public string? PrimaryKeyPhrase { get; set; }
    public PostFormat PostFormat { get; set; } = 0;
    public int WordCount { get; set; }
    public bool UseEmojis { get; set; } = true;
    public string? AdditionalPrompt { get; set; }
}
