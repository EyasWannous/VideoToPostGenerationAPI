using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class VideoRepository(AppDbContext context) : BaseRepository<Video>(context), IVideoRepository
{
    public async Task<IEnumerable<Video>> GetAllByUserIdAsync(int userId)
        => await _context.Audios
            .Where(audio => audio.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Video?> GetByIdToDeleteAsync(int id)
        => await _context.Audios
            .Where(audio => audio.Id == id)
            .Include("Video")
            .Include("Posts")
            .Include("Images")
            .FirstAsync();
}
