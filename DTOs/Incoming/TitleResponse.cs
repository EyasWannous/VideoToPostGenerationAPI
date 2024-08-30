using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Incoming;


/// <summary>
/// Represents the data transfer object for a title response.
/// </summary>
public record TitleResponse
{
    /// <summary>
    /// Gets or sets the title associated with the title response.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
}
