using Microsoft.AspNetCore.Identity;

namespace VideoToPostGenerationAPI.Domain.Entities;

public class User : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<Audio> Audios { get; set; } = [];
}