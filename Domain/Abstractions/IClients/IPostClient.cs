namespace VideoToPostGenerationAPI.Domain.Abstractions.IClients;

public interface IPostClient
{
    Task ReceiveMessageAsync(string message);

    Task SendPostRAsync(string message);

    Task SendMessageToCallerAsync(string user, string message);

    Task SendPostRAsync(string user, string message);
}
