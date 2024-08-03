using System.Text.Json.Serialization;

namespace VideoToPostGenerationAPI.DTOs.Incoming;

/// <summary>
/// Represents the response data for a transcription request.
/// </summary>
public record TranscriptionResponse
{
    /// <summary>
    /// Gets or sets the transcribed text from the audio or video.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
