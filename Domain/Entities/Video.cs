using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

/// <summary>
/// Represents a video file entity.
/// </summary>
public class Video : BaseFile
{
    /// <summary>
    /// Gets or sets the video file extension.
    /// </summary>
    public string VideoExtension { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the associated audio.
    /// </summary>
    public int AudioId { get; set; }

    /// <summary>
    /// Gets or sets the audio associated with this video.
    /// </summary>
    public Audio Audio { get; set; }
}
