namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record ResponseUploadFile
{
    public string Message { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
}
