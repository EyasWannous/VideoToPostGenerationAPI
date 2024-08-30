using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

/// <summary>
/// Interface for repository operations related to <see cref="PostImage"/> entities.
/// </summary>
public interface IPostImageRepository : IBaseRepository<PostImage>
{
    // This interface inherits all the methods from IBaseRepository<Image>
    // and can be extended with additional methods specific to the Image entity if needed.
}
