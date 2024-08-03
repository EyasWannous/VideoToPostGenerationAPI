using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

/// <summary>
/// Represents an image file entity.
/// </summary>
public class Image : BaseFile
{
    /// <summary>
    /// Gets or sets the image file extension.
    /// </summary>
    public ImageExtension ImageExtension { get; set; }

    /// <summary>
    /// Gets or sets the ID of the associated post.
    /// </summary>
    public int PostId { get; set; }

    /// <summary>
    /// Gets or sets the post associated with this image.
    /// </summary>
    public Post Post { get; set; }
}
