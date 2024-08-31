namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record ResponseAudioDTO
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public long SizeBytes { get; set; }

    public string Link { get; set; } = string.Empty;

    public string YoutubeLink { get; set; } = string.Empty;

    public string AudioExtension { get; set; } = string.Empty;

    public int Duration { get; set; }

    public ResponseVideoThumbnailDTO VideoThumbnail { get; set; }

    public bool HasVideo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
