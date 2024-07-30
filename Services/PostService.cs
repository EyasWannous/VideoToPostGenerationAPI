using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.DTOs.Incoming;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Services;

public class PostService : IPostService
{
    private bool _disposed = false;
    //private const string BaseURL = "http://127.0.0.1:8000/";
    private const string BaseURL = "http://192.168.1.9:8000/api/";
    private readonly HttpClient _client;

    public PostService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri(BaseURL);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<PostResponse?> GetPostAsync(PostRequest post, string paltform)
    {
        var json = JsonSerializer.Serialize(post);
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseURL}{paltform}");

        var content = new StringContent(json, null, "application/json");
        
        request.Content = content;
        
        var response = await _client.SendAsync(request);
        
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode)
            return null;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PostResponse>(responseContent);
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
