using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

/// <summary>
/// Represents the data transfer object for post response data.
/// </summary>
public record ResponsePostDTO
{
    /// <summary>
    /// Gets or sets the unique identifier for the post.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the description of the post.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    public double Rate { get; set; }

    /// <summary>
    /// Gets or sets the platform where the post is shared.
    /// </summary>
    public ResponsePostOptionsDTO PostOptions { get; set; }

    /// <summary>
    /// Gets or sets the header information associated with the post.
    /// This property may be null if the post does not have an associated header.
    /// </summary>
    public ResponseHeaderDTO? Header { get; set; }

    public List<ResponsePostImageDTO> PostImages { get; set; } = [];
    /// <summary>
    /// Gets or sets the date and time when the post was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the post was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
