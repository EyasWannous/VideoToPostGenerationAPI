using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class PostOptions : BaseEntity
{
    public string Platform { get; set; } = string.Empty;
    public PointOfView PointOfView { get; set; }
    //public string PrimaryKeyPhrase { get; set; } = string.Empty;
    public PostFormat PostFormat { get; set; }
    public bool UseEmojis { get; set; }
    public string AdditionalPrompt { get; set; } = string.Empty;
    public int WordCount { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
}
