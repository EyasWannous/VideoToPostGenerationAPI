namespace VideoToPostGenerationAPI.Domain.Entities;

public class VideoThumbnail : BaseEntity
{
    public string Link { get; set; }
    public int AudioId { get; set; }
    public Audio Audio { get; set; }
}
