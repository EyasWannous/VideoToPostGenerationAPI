using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

/// <summary>
/// Represents the data transfer object for a post request.
/// </summary>
public record PostRequest
{
    /// <summary>
    /// Gets or sets the script or content of the post.
    /// </summary>
    [JsonPropertyName("script")]
    public string Script { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the link or URL associated with the post.
    /// </summary>
    [JsonPropertyName("link")]
    public string Link { get; set; } = string.Empty;
}
