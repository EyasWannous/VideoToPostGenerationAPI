using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class Image : BaseFile
{
    public ImageExtension ImageExtension { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
}
