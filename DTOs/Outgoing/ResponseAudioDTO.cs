namespace VideoToPostGenerationAPI.DTOs.Outgoing;

/// <summary>
/// Represents the data transfer object for audio response data.
/// </summary>
public record ResponseAudioDTO
{
    /// <summary>
    /// Gets or sets the unique identifier for the audio.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the audio.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the audio file in bytes.
    /// </summary>
    public long SizeBytes { get; set; }

    /// <summary>
    /// Gets or sets the link or path to the audio file.
    /// </summary>
    public string Link { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the YouTube link associated with the audio.
    /// </summary>
    public string YoutubeLink { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the audio file extension.
    /// </summary>
    public string AudioExtension { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the duration of the audio in seconds.
    /// </summary>
    public int Duration { get; set; }

    public ResponseVideoThumbnailDTO VideoThumbnail { get; set; }

    public bool HasVideo { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the audio was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the audio was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
