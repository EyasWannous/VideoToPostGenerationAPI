using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class PostRepository(AppDbContext context) : BaseRepository<Post>(context), IPostRepository
{
    public async Task<IEnumerable<Post>> GetAllByVideoIdAsync(int videoId)
        => await _context.Posts
            .Where(post => post.VideoId == videoId)
            .Include("Images")
            .AsNoTracking()
            .ToListAsync();
}
