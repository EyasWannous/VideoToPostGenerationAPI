using VideoToPostGenerationAPI.DTOs.Incoming;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

/// <summary>
/// Defines operations for interacting with the Whisper service to provide transcription capabilities.
/// </summary>
public interface IWhisperService : IDisposable
{
    /// <summary>
    /// Retrieves the transcription of the audio file located at the specified file path.
    /// </summary>
    /// <param name="filePath">The path to the audio file for which transcription is requested.</param>
    /// <returns>A <see cref="TranscriptionResponse"/> object containing the transcription of the audio file, or null if the transcription request fails.</returns>
    /// <remarks>
    /// This method sends the audio file to the Whisper service and returns the transcription result.
    /// 
    /// Sample usage:
    /// 
    ///     var transcription = await whisperService.GetTranscriptAsync("path/to/audio/file.mp3");
    /// </remarks>
    Task<TranscriptionResponse?> GetTranscriptAsync(string filePath);
}
