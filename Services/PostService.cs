using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.DTOs.Incoming;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Services;

/// <summary>
/// Service for generating posts for different platforms.
/// </summary>
public class PostService : IPostService, IDisposable
{
    private bool _disposed = false;
    private const string BaseURL = "http://192.168.1.3:8000/api/";
    private readonly HttpClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostService"/> class.
    /// </summary>
    /// <param name="client">The HTTP client used for making requests.</param>
    public PostService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri(BaseURL);
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Generates a post for the specified platform based on the provided post request.
    /// </summary>
    /// <param name="post">The post request containing details for the post generation.</param>
    /// <param name="platform">The platform for which the post is to be generated.</param>
    /// <returns>A <see cref="PostResponse"/> object containing the generated post details, or null if the generation fails.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/blog
    ///     {
    ///         "Link": "https://example.com",
    ///         "Script": "This is the post content."
    ///     }
    /// 
    /// </remarks>
    public async Task<PostResponse?> GetPostAsync(PostRequest post, string platform)
    {
        var json = JsonSerializer.Serialize(post);
        var request = new HttpRequestMessage(HttpMethod.Post, $"{platform}");

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        request.Content = content;

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return null;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PostResponse>(responseContent);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="PostService"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">A boolean value indicating whether to release both managed and unmanaged resources.</param>
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
    /// Releases all resources used by the current instance of the <see cref="PostService"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
