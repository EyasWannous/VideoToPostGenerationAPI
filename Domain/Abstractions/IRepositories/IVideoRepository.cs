using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

/// <summary>
/// Interface for video repository operations.
/// </summary>
public interface IVideoRepository : IBaseRepository<Video>
{
    /// <summary>
    /// Gets all videos associated with the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose videos are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, containing a collection of videos.</returns>
    Task<IEnumerable<Video>> GetAllByUserIdAsync(int userId);

    /// <summary>
    /// Gets a video by its ID.
    /// </summary>
    /// <param name="id">The ID of the video.</param>
    /// <returns>A task representing the asynchronous operation, containing the video if found; otherwise, null.</returns>
    new Task<Video?> GetByIdAsync(int id);
}
