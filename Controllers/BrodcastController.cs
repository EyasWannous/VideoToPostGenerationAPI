using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;
using System.Text.Json;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IClients;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.DTOs.Incoming;
using VideoToPostGenerationAPI.DTOs.Outgoing;
using VideoToPostGenerationAPI.Presistence.Hubs;

namespace VideoToPostGenerationAPI.Controllers;

/// <summary>
/// Controller for handling broadcasting and receiving messages via SignalR and WebSocket.
/// </summary>
[Authorize]
public class BroadcastController : BaseController
{
    private readonly IHubContext<PostHub, IPostClient> _hubContext;
    private readonly UserManager<User> _userManager;
    private readonly IWebSocketClientService _webSocketClientServiceR;

    /// <summary>
    /// Initializes a new instance of the <see cref="BroadcastController"/> class.
    /// </summary>
    /// <param name="hubContext">The hub context for SignalR communication.</param>
    /// <param name="webSocketClientServiceR">The WebSocket client service.</param>
    /// <param name="userManager">The user manager service.</param>
    /// <param name="unitOfWork">The unit of work for data access.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public BroadcastController(IHubContext<PostHub, IPostClient> hubContext,
        IWebSocketClientService webSocketClientServiceR, UserManager<User> userManager,
        IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _hubContext = hubContext;
        _userManager = userManager;
        _webSocketClientServiceR = webSocketClientServiceR;
    }

    /// <summary>
    /// Broadcasts a message to the logged-in user via SignalR.
    /// </summary>
    /// <param name="message">The message to broadcast.</param>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    /// <response code="204">No content if the broadcast is successful.</response>
    [HttpPost("broadcast")]
    public async Task<IActionResult> BroadcastMessage(string message)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        await _hubContext.Clients.User(loggedinUser!.Id.ToString()).SendPostRAsync(message);
        return NoContent();
    }

    /// <summary>
    /// Receives messages from a WebSocket and broadcasts them to the logged-in user via SignalR.
    /// </summary>
    /// <returns>An action result containing the list of received messages.</returns>
    /// <response code="200">Returns the list of received messages.</response>
    /// <response code="500">If there is an internal server error during the WebSocket operations.</response>
    [HttpGet("receive")]
    public async Task<IActionResult> ReceiveMessage()
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);
        var userId = loggedinUser!.Id.ToString();

        var audios = await _unitOfWork.Audios.GetAllByUserIdAsync(loggedinUser.Id);
        var audio = audios.FirstOrDefault();

        var postRequest = new PostRequest
        {
            Link = audio?.YoutubeLink,
            Script = audio?.Transcript ?? "No Transcript",
            PostOptionsRequest = new PostOptionsRequest { Platform = ""},
        };

        var json = JsonSerializer.Serialize(postRequest);

        await _webSocketClientServiceR.ConnectAsync("ws://192.168.1.3:8000/ws");
        await _webSocketClientServiceR.SendMessageAsync(json);

        List<string> receivedMessages = new();

        while (_webSocketClientServiceR.State == WebSocketState.Open)
        {
            var receivedMessage = await _webSocketClientServiceR.ReceiveMessageAsync();
            receivedMessages.Add(receivedMessage);
            await _hubContext.Clients.User(userId).SendPostRAsync(receivedMessage);
        }

        await _webSocketClientServiceR.CloseAsync();

        return Ok(receivedMessages.Select(message => JsonSerializer.Deserialize<PostResponse>(message)).ToList());
    }
}
