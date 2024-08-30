using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

/// <summary>
/// Represents the data transfer object for a title request.
/// </summary>
public class TitleRequest
{
    /// <summary>
    /// Gets or sets the script associated with the title request.
    /// </summary>
    [JsonPropertyName("script")]
    public string Script { get; set; } = string.Empty;
}
