using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

public interface IPostImageRepository : IBaseRepository<PostImage>
{
    // This interface inherits all the methods from IBaseRepository<Image>
    // and can be extended with additional methods specific to the Image entity if needed.
}
