using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class Video : BaseFile
{
    public string VideoExtension { get; set; } = string.Empty;
    public int AudioId { get; set; }
    public Audio Audio { get; set; }
}
