namespace VideoToPostGenerationAPI.Domain.Entities;

public class Header : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public int PostId { get; set; }

    public Post Post { get; set; }
}
