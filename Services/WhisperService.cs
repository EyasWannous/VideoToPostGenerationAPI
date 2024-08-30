using System.Net.Http.Headers;
using System.Text.Json;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.DTOs.Incoming;

namespace VideoToPostGenerationAPI.Services;

/// <summary>
/// Service for interacting with the Whisper API to get transcriptions of audio files.
/// </summary>
public class WhisperService : IWhisperService, IDisposable
{
    private bool _disposed = false;
    private const string WhisperURL = "http://127.0.0.1:8000/api/whisper";
    private readonly HttpClient _client;
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Initializes a new instance of the <see cref="WhisperService"/> class.
    /// </summary>
    /// <param name="env">The environment in which the application is running.</param>
    /// <param name="client">The HTTP client used to send requests.</param>
    public WhisperService(IWebHostEnvironment env, HttpClient client)
    {
        _env = env;
        _client = client;
        _client.BaseAddress = new Uri(WhisperURL);
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.Timeout = TimeSpan.FromMinutes(30);
    }

    /// <summary>
    /// Gets the transcription of the audio file at the specified file path.
    /// </summary>
    /// <param name="filePath">The path to the audio file.</param>
    /// <returns>A <see cref="TranscriptionResponse"/> object containing the transcription, or null if the request fails.</returns>
    /// <remarks>
    /// Sample usage:
    ///
    ///     var transcription = await whisperService.GetTranscriptAsync("path/to/audio/file.mp3");
    ///
    /// </remarks>
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

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="WhisperService"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="WhisperService"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
