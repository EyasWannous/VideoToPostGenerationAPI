using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record ResponseVideoDTO
{
    public int Id { get; set; }

    public long SizeBytes { get; set; }

    public string Link { get; set; } = string.Empty;

    public string VideoExtension { get; set; } = string.Empty;

    public ResponseAudioDTO Audio { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
