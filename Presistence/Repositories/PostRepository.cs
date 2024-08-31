using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class PostRepository : BaseRepository<Post>, IPostRepository
{
    public PostRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Post>> GetAllByAudioIdAsync(int audioId)
    {
        return await _context.Posts
            .Where(post => post.AudioId == audioId)
            .Include(post => post.Header) // Using lambda expressions for includes
            .Include(post => post.PostImages)
            .Include(post => post.PostOptions)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Post?> GetByPostIdAsync(int postId)
    {
        return await _context.Posts
            .Where(post => post.Id == postId)
            .Include(post => post.Header)
            .Include(post => post.PostImages)
            .Include(post => post.PostOptions)
            //.AsNoTracking()
            .FirstOrDefaultAsync();
    }
}
