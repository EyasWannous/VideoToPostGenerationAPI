using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using VideoToPostGenerationAPI.Domain.Abstractions.IClients;

namespace VideoToPostGenerationAPI.Presistence.Hubs;

[Authorize]
public sealed class PostHub : Hub<IPostClient>
{
    public override async Task OnConnectedAsync()
    {
        // Notify all clients that a new connection has been established
        await Clients.All.ReceiveMessageAsync($"{Context.ConnectionId} has joined");
    }

    public async Task SendPostRAsync(string message, string userId)
    {
        // Send a message to the specified user
        await Clients.User(userId).SendPostRAsync(message);
    }

    public async Task SendMessageToCallerAsync(string user, string message)
    {
        // Send a message to the caller (the current connected user)
        await Clients.Caller.SendPostRAsync(user, message);
    }
}
