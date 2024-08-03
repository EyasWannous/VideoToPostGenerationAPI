using Microsoft.AspNetCore.Identity;

namespace VideoToPostGenerationAPI.Domain.Entities;

/// <summary>
/// Represents a user entity with identity properties and associated audio files.
/// </summary>
public class User : IdentityUser<int>
{
    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the user was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the list of audio files associated with the user.
    /// </summary>
    public List<Audio> Audios { get; set; } = new();
}
