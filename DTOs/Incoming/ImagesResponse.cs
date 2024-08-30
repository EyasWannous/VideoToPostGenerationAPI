using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Incoming;

public record ImagesResponse
{
    [JsonPropertyName("images")]
    public List<string> Images { get; set; } = [];
}
