using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class Audio : BaseFile
{
    public string AudioExtension { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string Transcript { get; set; } = string.Empty;
    public string YoutubeLink { get; set; } = string.Empty;
    public Video? Video { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<Post> Posts { get; set; } = [];
}
