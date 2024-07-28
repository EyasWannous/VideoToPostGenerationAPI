using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record ResponseAudio
{
    public int Id { get; set; }
    public long SizeBytes { get; set; }
    public string Link { get; set; } = string.Empty;
    public string YoutubeLink { get; set; } = string.Empty;
    public string AudioExtension { get; set; } = string.Empty;
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
