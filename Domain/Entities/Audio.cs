using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class Audio : BaseFile
{
    public string AudioExtension { get; set; } = string.Empty;
    public int VideoId { get; set; }
    public Video Video { get; set; }
}
