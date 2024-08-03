using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

/// <summary>
/// Represents the data transfer object for video response data.
/// </summary>
public record ResponseVideoDTO
{
    /// <summary>
    /// Gets or sets the unique identifier for the video.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the size of the video file in bytes.
    /// </summary>
    public long SizeBytes { get; set; }

    /// <summary>
    /// Gets or sets the link or path to the video file.
    /// </summary>
    public string Link { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the video file extension.
    /// </summary>
    public string VideoExtension { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the audio response data associated with the video.
    /// This property is required and provides information about the audio linked to the video.
    /// </summary>
    public ResponseAudioDTO Audio { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the video was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the video was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
