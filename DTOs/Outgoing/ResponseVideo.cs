using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record ResponseVideo
{
    public int Id { get; set; }
    public long SizeBytes { get; set; }
    public string Link { get; set; } = string.Empty;
    public VideoExtension VideoExtension { get; set; }
    public ResponseAudio Audio { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
