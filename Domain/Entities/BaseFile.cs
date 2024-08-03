namespace VideoToPostGenerationAPI.Domain.Entities;

/// <summary>
/// Represents a base file entity with common file properties.
/// </summary>
public class BaseFile : BaseEntity
{
    /// <summary>
    /// Gets or sets the size of the file in bytes.
    /// </summary>
    public long SizeBytes { get; set; }

    /// <summary>
    /// Gets or sets the link or path to the file.
    /// </summary>
    public string Link { get; set; } = string.Empty;

    // Uncomment and add XML comments if you plan to use the File property.
    // /// <summary>
    // /// Gets or sets the file associated with this entity.
    // /// </summary>
    // public File File { get; set; }
}
