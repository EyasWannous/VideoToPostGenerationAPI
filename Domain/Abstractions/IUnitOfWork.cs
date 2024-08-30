using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

namespace VideoToPostGenerationAPI.Domain.Abstractions;

/// <summary>
/// Interface for the Unit of Work pattern. This interface coordinates the work of multiple repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository for user-related operations.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Gets the repository for audio-related operations.
    /// </summary>
    IAudioRepository Audios { get; }

    /// <summary>
    /// Gets the repository for video-related operations.
    /// </summary>
    IVideoRepository Videos { get; }

    /// <summary>
    /// Gets the repository for header-related operations.
    /// </summary>
    IHeaderRepository Headers { get; }

    /// <summary>
    /// Gets the repository for post-related operations.
    /// </summary>
    IPostRepository Posts { get; }

    /// <summary>
    /// Gets the repository for image-related operations.
    /// </summary>
    IPostImageRepository PostsImages { get; }

    IPostOptionsRepository PostsOptions { get; }

    IVideoThumbnailRepository VideoThumbnails { get; }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> CompleteAsync();
}
