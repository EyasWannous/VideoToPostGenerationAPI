using System.Net.WebSockets;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

/// <summary>
/// Defines the service for handling WebSocket client operations.
/// </summary>
public interface IWebSocketClientService
{
    /// <summary>
    /// Connects to the WebSocket server at the specified URL.
    /// </summary>
    /// <param name="url">The URL of the WebSocket server to connect to.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ConnectAsync(string url);

    /// <summary>
    /// Sends a message to the connected WebSocket server.
    /// </summary>
    /// <param name="message">The message to be sent to the server.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendMessageAsync(string message);

    /// <summary>
    /// Receives a message from the connected WebSocket server.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with a string result containing the received message.</returns>
    Task<string> ReceiveMessageAsync();

    /// <summary>
    /// Closes the connection to the WebSocket server.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CloseAsync();

    /// <summary>
    /// Gets the current state of the WebSocket connection.
    /// </summary>
    WebSocketState State { get; }
}
