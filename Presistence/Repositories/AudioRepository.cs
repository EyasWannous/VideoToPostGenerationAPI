using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class AudioRepository : BaseRepository<Audio>, IAudioRepository
{
    public AudioRepository(AppDbContext context) : base(context)
    {
    }

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

    public async Task<Audio?> GetByIdToDeleteAsync(int id)
        => await _context.Audios
            .Where(audio => audio.Id == id)
            .Include(audio => audio.Video)
            .Include(audio => audio.Posts)
            .ThenInclude(post => post.PostImages)
            .FirstOrDefaultAsync(); // Changed to FirstOrDefaultAsync for better null handling
}
