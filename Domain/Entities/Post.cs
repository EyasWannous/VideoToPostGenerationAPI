using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class Post : BaseEntitiy
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Platform Platform { get; set; }
    public int GeneralPostId { get; set; }
    public GeneralPost GeneralPost { get; set; }
    public List<Image> Images { get; set; } = [];
}
