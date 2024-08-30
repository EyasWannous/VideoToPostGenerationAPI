using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

/// <summary>
/// Repository for handling audio-related data operations.
/// </summary>
public class AudioRepository : BaseRepository<Audio>, IAudioRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public AudioRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves all audios for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A collection of audios associated with the specified user.</returns>
    public async Task<IEnumerable<Audio>> GetAllByUserIdAsync(int userId)
        => await _context.Audios
            .Where(audio => audio.UserId == userId)
            .Include(audio => audio.VideoThumbnail)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Audio?> GetAudioByIdForPost(int id)
        => await _context.Audios
            .Where(audio => audio.Id == id)
            .Include(audio => audio.Video)
            .FirstOrDefaultAsync();

    /// <summary>
    /// Retrieves an audio entity by its ID, including related data, for deletion.
    /// </summary>
    /// <param name="id">The ID of the audio entity.</param>
    /// <returns>The audio entity, or null if not found.</returns>
    public async Task<Audio?> GetByIdToDeleteAsync(int id)
        => await _context.Audios
            .Where(audio => audio.Id == id)
            .Include(audio => audio.Video)
            .Include(audio => audio.Posts)
            .ThenInclude(post => post.PostImages)
            .FirstOrDefaultAsync(); // Changed to FirstOrDefaultAsync for better null handling
}
