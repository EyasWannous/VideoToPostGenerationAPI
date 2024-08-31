using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

public interface IAudioRepository : IBaseRepository<Audio>
{
    Task<IEnumerable<Audio>> GetAllByUserIdAsync(int userId);

    Task<Audio?> GetByIdToDeleteAsync(int id);

    Task<Audio?> GetAudioByIdForPost(int id);
}
