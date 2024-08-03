using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

/// <summary>
/// Repository for handling operations related to <see cref="Video"/> entities.
/// </summary>
public class VideoRepository : BaseRepository<Video>, IVideoRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VideoRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public VideoRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves all videos associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A collection of videos associated with the user.</returns>
    public async Task<IEnumerable<Video>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Videos
            .Where(video => video.Audio.UserId == userId)
            .Include(video => video.Audio)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a video by its ID.
    /// </summary>
    /// <param name="id">The ID of the video.</param>
    /// <returns>The video with the specified ID or null if not found.</returns>
    public new async Task<Video?> GetByIdAsync(int id)
    {
        return await _context.Videos
            .Where(video => video.Id == id)
            .Include(video => video.Audio)
            .FirstOrDefaultAsync();
    }
}
