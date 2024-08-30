namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record ResponsePostImageDTO
{
    public string ImageExtension { get; set; } = string.Empty;

    public long SizeBytes { get; set; }

    public string Link { get; set; } = string.Empty;
}
