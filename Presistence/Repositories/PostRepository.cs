using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

/// <summary>
/// Repository for handling operations related to <see cref="Post"/> entities.
/// </summary>
public class PostRepository : BaseRepository<Post>, IPostRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public PostRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves all posts associated with a specific audio ID.
    /// </summary>
    /// <param name="audioId">The audio ID to filter posts.</param>
    /// <returns>A collection of <see cref="Post"/> entities that match the specified audio ID.</returns>
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
