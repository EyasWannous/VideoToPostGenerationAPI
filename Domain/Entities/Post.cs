
using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class Post : BaseEntitiy
{
    public string Description { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public int AudioId { get; set; }
    public Audio Audio { get; set; }
    public Header? Header { get; set; }
    public List<Image> Images { get; set; } = [];
}
