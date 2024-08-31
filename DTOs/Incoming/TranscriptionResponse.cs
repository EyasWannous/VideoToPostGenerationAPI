using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Incoming;

public record TranscriptionResponse
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
