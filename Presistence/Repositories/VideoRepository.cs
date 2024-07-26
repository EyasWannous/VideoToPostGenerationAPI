using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class VideoRepository(AppDbContext context) : BaseRepository<Video>(context) , IVideoRepository
{
    public async Task<IEnumerable<Video>> GetAllByUserIdAsync(int userId)
    => await _context.Videos
        .Where(video => video.Audio.UserId == userId)
        .Include("Audio")
        .AsNoTracking()
        .ToListAsync();

    public new async Task<Video?> GetByIdAsync(int id)
    => await _context
        .Videos
        .Where(video => video.Id == id)
        .Include("Audio")
        .FirstAsync();
}
