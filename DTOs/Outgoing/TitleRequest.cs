using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public class TitleRequest
{
    [JsonPropertyName("script")]
    public string Script { get; set; } = string.Empty;
}
