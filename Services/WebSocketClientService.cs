using System.Net.WebSockets;
using System.Text;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;

namespace VideoToPostGenerationAPI.Services;

public class WebSocketClientService : IWebSocketClientService
{
    private readonly ClientWebSocket _client;

    public WebSocketClientService(ClientWebSocket client)
    {
        _client = client;
    }

    public async Task ConnectAsync(string url)
    {
        await _client.ConnectAsync(new Uri(url), CancellationToken.None);
    }

    public async Task SendMessageAsync(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        await _client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async Task<string> ReceiveMessageAsync()
    {
        var buffer = new byte[1024 * 4];
        var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        return Encoding.UTF8.GetString(buffer, 0, result.Count);
    }

    public async Task CloseAsync()
    {
        await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the client", CancellationToken.None);
    }

    public WebSocketState State => _client.State;
}
