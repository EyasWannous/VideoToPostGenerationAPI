using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

public interface IVideoRepository : IBaseRepository<Video>
{
    Task<IEnumerable<Video>> GetAllByUserIdAsync(int userId);
    Task<Video?> GetByIdToDeleteAsync(int id);
}
