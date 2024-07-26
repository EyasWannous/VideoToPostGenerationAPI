using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class AudioRepository(AppDbContext context) : BaseRepository<Audio>(context), IAudioRepository
{
    public async Task<IEnumerable<Audio>> GetAllByUserIdAsync(int userId)
        => await _context.Audios
            .Where(audio => audio.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
}
