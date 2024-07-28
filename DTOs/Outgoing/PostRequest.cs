using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record PostRequest
{

    [JsonPropertyName("script")]
    public string Script { get; set; } = string.Empty;

    [JsonPropertyName("link")]
    public string Link { get; set; } = string.Empty;
}
