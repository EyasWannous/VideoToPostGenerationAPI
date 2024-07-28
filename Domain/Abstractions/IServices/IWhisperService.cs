using VideoToPostGenerationAPI.DTOs.Incoming;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IWhisperService : IDisposable
{
    public Task<TranscriptionResponse?> GetTranscriptAsync(string filePath);
}
