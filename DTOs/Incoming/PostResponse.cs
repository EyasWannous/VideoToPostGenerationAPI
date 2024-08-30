using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Incoming;

/// <summary>
/// Represents the data transfer object for a post response.
/// </summary>
public record PostResponse
{
    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    [JsonPropertyName("post")]
    public string Post { get; set; } = string.Empty;

    [JsonPropertyName("rating")]
    public double Rate {  get; set; }
 
    [JsonPropertyName("images")]
    public List<string> Images { get; set; } = [];
}
