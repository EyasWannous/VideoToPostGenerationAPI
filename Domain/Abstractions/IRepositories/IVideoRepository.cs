using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

public interface IVideoRepository : IBaseRepository<Video>
{
    Task<IEnumerable<Video>> GetAllByUserIdAsync(int userId);
    public new Task<Video?> GetByIdAsync(int id);
}
