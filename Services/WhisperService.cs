using System.Net.Http.Headers;
using System.Text.Json;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.DTOs.Incoming;

namespace VideoToPostGenerationAPI.Services;

public class WhisperService : IWhisperService
{
    private bool _disposed = false;
    private const string WhisperURL = "http://127.0.0.1:8000/api/whisper";
    private readonly HttpClient _client;
    private readonly IWebHostEnvironment _env;

    public WhisperService(IWebHostEnvironment env, HttpClient client)
    {
        _env = env;
        _client = client;
        _client.BaseAddress = new Uri(WhisperURL);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.Timeout = TimeSpan.FromSeconds(300);
    }

    public async Task<TranscriptionResponse?> GetTranscriptAsync(string filePath)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "");

        var content = new MultipartFormDataContent();

        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        var stream = new StreamContent(File.OpenRead(fullPath));

        content.Add(stream, "file", filePath.Split('\\').Last());

        request.Content = content;

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode)
            return null;

        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TranscriptionResponse>(responseContent);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _client.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }
}
