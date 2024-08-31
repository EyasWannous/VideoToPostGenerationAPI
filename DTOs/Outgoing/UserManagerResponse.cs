using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record UserManagerResponse
{
    public User? User { get; set; }

    public string Message { get; set; } = string.Empty;

    public bool IsSuccess { get; set; }

    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

    public DateTime? ExpiryDate { get; set; }
}
