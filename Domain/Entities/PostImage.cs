using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class PostImage : BaseFile
{
    public string ImageExtension { get; set; } = string.Empty;

    public int PostId { get; set; }

    public Post Post { get; set; }
}
