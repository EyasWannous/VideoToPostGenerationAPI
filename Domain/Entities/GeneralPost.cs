namespace VideoToPostGenerationAPI.Domain.Entities;

public class GeneralPost : BaseEntitiy
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AudioId { get; set; }
    public Audio Audio { get; set; }
    public List<Post> Posts { get; set; } = [];
}
