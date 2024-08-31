using System.Net.WebSockets;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IWebSocketClientService
{
    Task ConnectAsync(string url);

    Task SendMessageAsync(string message);

    Task<string> ReceiveMessageAsync();

    Task CloseAsync();

    WebSocketState State { get; }
}
