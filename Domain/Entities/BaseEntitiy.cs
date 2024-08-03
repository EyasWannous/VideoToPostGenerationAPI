namespace VideoToPostGenerationAPI.Domain.Entities;

/// <summary>
/// Represents the base entity with common properties.
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the entity was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
