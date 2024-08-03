using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

/// <summary>
/// Interface for accessing and managing audio data in the repository.
/// </summary>
public interface IAudioRepository : IBaseRepository<Audio>
{
    /// <summary>
    /// Retrieves all audio records associated with the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose audio records are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of <see cref="Audio"/> entities associated with the specified user ID.</returns>
    Task<IEnumerable<Audio>> GetAllByUserIdAsync(int userId);

    /// <summary>
    /// Retrieves an audio record by its ID, intended for deletion purposes.
    /// </summary>
    /// <param name="id">The ID of the audio record to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="Audio"/> entity if found, or <c>null</c> if no record with the specified ID exists.</returns>
    Task<Audio?> GetByIdToDeleteAsync(int id);
}
