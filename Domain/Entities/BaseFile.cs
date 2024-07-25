namespace VideoToPostGenerationAPI.Domain.Entities;

public class BaseFile : BaseEntitiy
{
    public long SizeBytes { get; set; }
    public string Link { get; set; } = string.Empty;
    //public File File { get; set; }
}
