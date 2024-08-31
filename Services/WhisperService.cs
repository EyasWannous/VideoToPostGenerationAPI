using System.Net.Http.Headers;
using System.Text.Json;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.DTOs.Incoming;

namespace VideoToPostGenerationAPI.Services;

public class WhisperService : IWhisperService, IDisposable
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
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.Timeout = TimeSpan.FromMinutes(30);
    }

    public async Task<TranscriptionResponse?> GetTranscriptAsync(string filePath)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "");

        var content = new MultipartFormDataContent();

        var fullPath = Path.Combine(_env.WebRootPath, filePath);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("The specified file was not found.", fullPath);
        }

        using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        var fileContent = new StreamContent(stream);

        content.Add(fileContent, "file", Path.GetFileName(filePath));

        request.Content = content;

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

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
