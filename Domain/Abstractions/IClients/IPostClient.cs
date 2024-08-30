namespace VideoToPostGenerationAPI.Domain.Abstractions.IClients;

/// <summary>
/// Defines the contract for a client that interacts with a SignalR hub for sending and receiving messages.
/// </summary>
public interface IPostClient
{
    /// <summary>
    /// Receives a message from the server.
    /// </summary>
    /// <param name="message">The message received from the server.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ReceiveMessageAsync(string message);

    /// <summary>
    /// Sends a post response message to the connected client.
    /// </summary>
    /// <param name="message">The post response message to be sent to the client.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendPostRAsync(string message);

    /// <summary>
    /// Sends a message to the caller, typically the current connected user.
    /// </summary>
    /// <param name="user">The user identifier of the caller.</param>
    /// <param name="message">The message to be sent to the caller.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendMessageToCallerAsync(string user, string message);

    /// <summary>
    /// Sends a post response message to a specific user.
    /// </summary>
    /// <param name="user">The user identifier of the recipient.</param>
    /// <param name="message">The post response message to be sent to the specific user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendPostRAsync(string user, string message);
}
