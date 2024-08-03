using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

/// <summary>
/// Represents a post entity containing a description, platform, associated audio, header, and images.
/// </summary>
public class Post : BaseEntity
{
    /// <summary>
    /// Gets or sets the description of the post.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the platform where the post is shared.
    /// </summary>
    public string Platform { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the associated audio.
    /// </summary>
    public int AudioId { get; set; }

    /// <summary>
    /// Gets or sets the audio associated with this post.
    /// </summary>
    public Audio Audio { get; set; }

    /// <summary>
    /// Gets or sets the header associated with this post.
    /// </summary>
    public Header? Header { get; set; }

    /// <summary>
    /// Gets or sets the list of images associated with this post.
    /// </summary>
    public List<Image> Images { get; set; } = new();
}
