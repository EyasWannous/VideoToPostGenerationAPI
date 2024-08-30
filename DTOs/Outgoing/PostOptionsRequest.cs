using System.Text.Json.Serialization;
using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record PostOptionsRequest
{
    [JsonPropertyName("platform")]
    public required string Platform { get; set; }
    [JsonPropertyName("point_of_view")]
    public string? PointOfView { get; set; }
    //[JsonPropertyName("primary_key_phrase")]
    //public string? PrimaryKeyPhrase { get; set; }
    [JsonPropertyName("post_format")]
    public string? PostFormat { get; set; }
    [JsonPropertyName("use_emojis")]
    public bool? UseEmojis { get; set; }
    [JsonPropertyName("additional_prompt")]
    public string? AdditionalPrompt { get; set; }
    [JsonPropertyName("word_count")]
    public int? WordCount { get; set; }
}
