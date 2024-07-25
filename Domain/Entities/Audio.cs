using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class Audio : BaseFile
{
    public AudioExtension AudioExtension { get; set; }
    public int Duration { get; set; }
    public Video? Video { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<GeneralPost> GeneralPosts { get; set; } = [];
}
