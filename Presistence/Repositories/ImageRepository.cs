using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

/// <summary>
/// Repository for handling operations related to <see cref="Image"/> entities.
/// </summary>
public class ImageRepository : BaseRepository<Image>, IImageRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ImageRepository(AppDbContext context) : base(context)
    {
    }
}
