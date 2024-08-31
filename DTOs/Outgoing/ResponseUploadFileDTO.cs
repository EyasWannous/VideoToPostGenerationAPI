namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record ResponseUploadFileDTO
{
    public string Message { get; set; } = string.Empty;

    public string Link { get; set; } = string.Empty;

    public bool IsSuccess { get; set; }

    public ResponseAudioDTO? Audio { get; set; }

    public ResponseVideoDTO? Video { get; set; }
}
