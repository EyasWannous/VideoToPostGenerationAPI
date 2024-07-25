using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class Video : BaseFile
{
    public VideoExtension VideoExtension { get; set; }
    public int AudioId { get; set; }
    public Audio Audio { get; set; }
}
