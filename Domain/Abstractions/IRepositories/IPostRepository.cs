using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

/// <summary>
/// Interface for repository operations related to <see cref="Post"/> entities.
/// </summary>
public interface IPostRepository : IBaseRepository<Post>
{
    /// <summary>
    /// Retrieves all <see cref="Post"/> entities associated with the specified audio ID.
    /// </summary>
    /// <param name="audioId">The ID of the audio associated with the posts.</param>
    /// <returns>A collection of <see cref="Post"/> entities associated with the specified audio ID.</returns>
    Task<IEnumerable<Post>> GetAllByAudioIdAsync(int audioId);
}
