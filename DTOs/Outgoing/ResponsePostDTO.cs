using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record ResponsePostDTO
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;
    public double Rate { get; set; }

    public ResponsePostOptionsDTO PostOptions { get; set; }

    public ResponseHeaderDTO? Header { get; set; }

    public List<ResponsePostImageDTO> PostImages { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
