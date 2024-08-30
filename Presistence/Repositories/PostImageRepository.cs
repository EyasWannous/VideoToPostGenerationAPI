using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

/// <summary>
/// Repository for handling operations related to <see cref="PostImage"/> entities.
/// </summary>
public class PostImageRepository : BaseRepository<PostImage>, IPostImageRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostImageRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public PostImageRepository(AppDbContext context) : base(context)
    {
    }
}
