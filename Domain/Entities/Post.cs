
using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class Post : BaseEntitiy
{
    public string Description { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public int VideoId { get; set; }
    public Video Video { get; set; }
    public Header? Header { get; set; }
    public List<Image> Images { get; set; } = [];
}
