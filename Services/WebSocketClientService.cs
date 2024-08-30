using System.Net.WebSockets;
using System.Text;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;

namespace VideoToPostGenerationAPI.Services;

/// <summary>
/// Implementation of a WebSocket client service for handling WebSocket communication.
/// </summary>
public class WebSocketClientService : IWebSocketClientService
{
    private readonly ClientWebSocket _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebSocketClientService"/> class.
    /// </summary>
    /// <param name="client">The <see cref="ClientWebSocket"/> instance used for WebSocket communication.</param>
    public WebSocketClientService(ClientWebSocket client)
    {
        _client = client;
    }

    /// <summary>
    /// Connects the WebSocket client to the specified URL.
    /// </summary>
    /// <param name="url">The URL to connect to.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ConnectAsync(string url)
    {
        await _client.ConnectAsync(new Uri(url), CancellationToken.None);
    }

    /// <summary>
    /// Sends a message to the WebSocket server.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendMessageAsync(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        await _client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    /// <summary>
    /// Receives a message from the WebSocket server.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the received message as a string.</returns>
    public async Task<string> ReceiveMessageAsync()
    {
        var buffer = new byte[1024 * 4];
        var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        return Encoding.UTF8.GetString(buffer, 0, result.Count);
    }

    /// <summary>
    /// Closes the WebSocket connection.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the client", CancellationToken.None);
    }

    /// <summary>
    /// Gets the current state of the WebSocket connection.
    /// </summary>
    public WebSocketState State => _client.State;
}
