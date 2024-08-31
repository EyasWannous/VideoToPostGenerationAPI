using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class Post : BaseEntity
{
    public string Description { get; set; } = string.Empty;

    //public string Platform { get; set; } = string.Empty;
    public double Rate { get; set; }

    public int AudioId { get; set; }

    public Audio Audio { get; set; }

    public Header? Header { get; set; }

    public List<PostImage> PostImages { get; set; } = [];

    public PostOptions PostOptions { get; set; }
}
