namespace VideoToPostGenerationAPI.Domain.Entities;

public class BaseFile : BaseEntity
{
    public long SizeBytes { get; set; }

    public string Link { get; set; } = string.Empty;

    // Uncomment and add XML comments if you plan to use the File property.
    // public File File { get; set; }
}
