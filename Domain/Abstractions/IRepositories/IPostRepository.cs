using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<IEnumerable<Post>> GetAllByVideoIdAsync(int videoId);
}
