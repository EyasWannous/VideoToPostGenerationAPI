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

[Authorize]
public class BroadcastController : BaseController
{
    private readonly IHubContext<PostHub, IPostClient> _hubContext;
    private readonly UserManager<User> _userManager;
    private readonly IWebSocketClientService _webSocketClientServiceR;

    public BroadcastController(IHubContext<PostHub, IPostClient> hubContext,
        IWebSocketClientService webSocketClientServiceR, UserManager<User> userManager,
        IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _hubContext = hubContext;
        _userManager = userManager;
        _webSocketClientServiceR = webSocketClientServiceR;
    }

    [HttpPost("broadcast")]
    public async Task<IActionResult> BroadcastMessage(string message)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        await _hubContext.Clients.User(loggedinUser!.Id.ToString()).SendPostRAsync(message);
        return NoContent();
    }

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
            PostOptionsRequest = new PostOptionsRequest { Platform = "" },
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
