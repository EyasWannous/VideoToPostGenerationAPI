using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class VideoRepository : BaseRepository<Video>, IVideoRepository
{
    public VideoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Video>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Videos
            .Where(video => video.Audio.UserId == userId)
            .Include(video => video.Audio)
            .AsNoTracking()
            .ToListAsync();
    }

    public new async Task<Video?> GetByIdAsync(int id)
    {
        return await _context.Videos
            .Where(video => video.Id == id)
            .Include(video => video.Audio)
            .FirstOrDefaultAsync();
    }
}
