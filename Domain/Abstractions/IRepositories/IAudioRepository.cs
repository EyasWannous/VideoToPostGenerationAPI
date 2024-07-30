using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

public interface IAudioRepository : IBaseRepository<Audio>
{
    Task<IEnumerable<Audio>> GetAllByUserIdAsync(int userId);
    public new Task<Audio?> GetByIdAsync(int id);
}
