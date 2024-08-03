namespace VideoToPostGenerationAPI.DTOs.Outgoing;

/// <summary>
/// Represents the data transfer object for header response data.
/// </summary>
public record ResponseHeaderDTO
{
    /// <summary>
    /// Gets or sets the title of the header.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the associated post.
    /// </summary>
    public int PostId { get; set; }
}
