using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using VideoToPostGenerationAPI.Domain.Abstractions.IClients;

namespace VideoToPostGenerationAPI.Presistence.Hubs;

/// <summary>
/// Hub for managing real-time communication with clients using SignalR.
/// </summary>
[Authorize]
public sealed class PostHub : Hub<IPostClient>
{
    /// <summary>
    /// Called when a connection is established with the hub.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task OnConnectedAsync()
    {
        // Notify all clients that a new connection has been established
        await Clients.All.ReceiveMessageAsync($"{Context.ConnectionId} has joined");
    }

    /// <summary>
    /// Sends a post response message to a specific user.
    /// </summary>
    /// <param name="message">The message to be sent.</param>
    /// <param name="userId">The ID of the user to receive the message.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendPostRAsync(string message, string userId)
    {
        // Send a message to the specified user
        await Clients.User(userId).SendPostRAsync(message);
    }

    /// <summary>
    /// Sends a message to the caller (the user who invoked the method).
    /// </summary>
    /// <param name="user">The user identifier to include in the message.</param>
    /// <param name="message">The message to be sent to the caller.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendMessageToCallerAsync(string user, string message)
    {
        // Send a message to the caller (the current connected user)
        await Clients.Caller.SendPostRAsync(user, message);
    }
}
