using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<IEnumerable<Post>> GetAllByAudioIdAsync(int audioId);
    Task<Post?> GetByPostIdAsync(int postId);
}
