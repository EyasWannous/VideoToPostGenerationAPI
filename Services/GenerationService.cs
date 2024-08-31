using Microsoft.AspNetCore.SignalR;
using Quartz.Util;
using System.Drawing;
using System;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using VideoToPostGenerationAPI.Domain.Abstractions.IClients;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.DTOs.Incoming;
using VideoToPostGenerationAPI.DTOs.Outgoing;
using VideoToPostGenerationAPI.Presistence.Hubs;

namespace VideoToPostGenerationAPI.Services;

public class GenerationService : IGenerationService
{
    private bool _disposed = false;
    private const string BaseURL = "http://127.0.0.1:8000/api/";
    private const string BaseWebSocketURL = "ws://192.168.1.9:8000/api/";
    private readonly HttpClient _client;
    private readonly IWebSocketClientService _webSocketClientServiceR;
    private readonly IHubContext<PostHub, IPostClient> _hubContext;
    private readonly IWebHostEnvironment _env;


    public GenerationService(HttpClient client, IHubContext<PostHub, IPostClient> hubContext, IWebSocketClientService webSocketClientServiceR, IWebHostEnvironment env)
    {
        _env = env;
        _client = client;
        _client.BaseAddress = new Uri(BaseURL);
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.Timeout = TimeSpan.FromMinutes(30);
        _hubContext = hubContext;
        _webSocketClientServiceR = webSocketClientServiceR;
    }

    public async Task<TitleResponse?> GetTitleAsync(TitleRequest titleRequest)
    {
        var json = JsonSerializer.Serialize(titleRequest);
        var request = new HttpRequestMessage(HttpMethod.Post, "title")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return null;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TitleResponse>(responseContent);
    }

    //public async Task<PostResponse?> GetPostAsync(PostRequest post, string platform)
    //{
    //    var json = JsonSerializer.Serialize(post);
    //    var request = new HttpRequestMessage(HttpMethod.Post, platform)
    //    {
    //        Content = new StringContent(json, Encoding.UTF8, "application/json")
    //    };

    //    var response = await _client.SendAsync(request);

    //    if (!response.IsSuccessStatusCode)
    //        return null;

    //    var responseContent = await response.Content.ReadAsStringAsync();
    //    return JsonSerializer.Deserialize<PostResponse>(responseContent);
    //}

    public async Task<List<PostResponse?>> GetPostWSAsync(string userId, PostRequest post, string platform)
    {
        var json = JsonSerializer.Serialize(post);

        await _webSocketClientServiceR.ConnectAsync($"{BaseWebSocketURL}{platform}");
        await _webSocketClientServiceR.SendMessageAsync(json);

        var receivedMessages = new List<string>();

        while (_webSocketClientServiceR.State == WebSocketState.Open)
        {
            var receivedMessage = await _webSocketClientServiceR.ReceiveMessageAsync();
            receivedMessages.Add(receivedMessage);
            await _hubContext.Clients.User(userId).SendPostRAsync(receivedMessage);
        }

        await _webSocketClientServiceR.CloseAsync();

        var posts = receivedMessages
            .Where(message => !message.IsNullOrWhiteSpace())
            .Select(message => JsonSerializer.Deserialize<PostResponse>(message))
            .ToList();

        return posts;
    }

    public async Task<PostResponse?> GetPostAsync(PostRequest post)
    {
        post.VideoLink = null;
        var postJson = JsonSerializer.Serialize(post);
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "generate")
        {
            Content = new StringContent(postJson, Encoding.UTF8, "application/json")
        };

        var postResponse = await _client.SendAsync(postRequest);

        if (!postResponse.IsSuccessStatusCode)
            return null;

        var responseContent = await postResponse.Content.ReadAsStringAsync();
        var postResult = JsonSerializer.Deserialize<PostResponse>(responseContent);

        if (postResult is null)
            return null;

        //if (post.VideoLink is not null)
        //{
        //    var imagesRequest = new HttpRequestMessage(HttpMethod.Post, "upload-video");
        //    var imagesContent = new MultipartFormDataContent();
        //    var fullPath = Path.Combine(_env.WebRootPath, post.VideoLink);
        //    imagesContent.Add(new StreamContent(File.OpenRead(fullPath)), "video_file", fullPath);


        //    imagesRequest.Content = imagesContent;

        //    var imagesResponse = await _client.SendAsync(imagesRequest);

        //    if (!imagesResponse.IsSuccessStatusCode)
        //        return null;

        //    var imagesResponseContent = await imagesResponse.Content.ReadAsStringAsync();

        //    var imagesResult = JsonSerializer.Deserialize<ImagesResponse>(imagesResponseContent);

        //    postResult.Images = imagesResult?.Images ?? [];
        //}

        return postResult;
    }

    public async Task<List<string>?> GetImagesForPost(PostRequest post)
    {
        var imagesRequest = new HttpRequestMessage(HttpMethod.Post, "upload-video");
        var imagesContent = new MultipartFormDataContent();
        var fullPath = Path.Combine(_env.WebRootPath, post.VideoLink!);
        imagesContent.Add(new StreamContent(File.OpenRead(fullPath)), "video_file", fullPath);


        imagesRequest.Content = imagesContent;

        var imagesResponse = await _client.SendAsync(imagesRequest);

        if (!imagesResponse.IsSuccessStatusCode)
            return null;

        var imagesResponseContent = await imagesResponse.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ImagesResponse>(imagesResponseContent)?.Images;
    }

    public async Task<bool> DeleteImageAsync(string imageName)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "image")
        {
            Content = new MultipartFormDataContent
            {
                { new StringContent(imageName), "name" }
            }
        };

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return false;

        return true;
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
