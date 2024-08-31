using VideoToPostGenerationAPI.DTOs.Incoming;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IWhisperService : IDisposable
{
    Task<TranscriptionResponse?> GetTranscriptAsync(string filePath);
}
