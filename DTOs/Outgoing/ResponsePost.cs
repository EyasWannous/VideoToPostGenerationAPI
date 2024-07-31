using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public class ResponsePost
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public Header? Header { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
