using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Incoming;


public record TitleResponse
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
}
