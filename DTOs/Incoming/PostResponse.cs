using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Incoming;

public record PostResponse
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("post")]
    public string Post { get; set; } = string.Empty;
}
