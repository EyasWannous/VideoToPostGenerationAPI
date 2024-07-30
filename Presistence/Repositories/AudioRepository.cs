using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class AudioRepository(AppDbContext context) : BaseRepository<Audio>(context) , IAudioRepository
{
    public async Task<IEnumerable<Audio>> GetAllByUserIdAsync(int userId)
    => await _context.Videos
        .Where(video => video.Video.UserId == userId)
        .Include("Audio")
        .AsNoTracking()
        .ToListAsync();

    public new async Task<Audio?> GetByIdAsync(int id)
    => await _context
        .Videos
        .Where(video => video.Id == id)
        .Include("Audio")
        .FirstAsync();
}
