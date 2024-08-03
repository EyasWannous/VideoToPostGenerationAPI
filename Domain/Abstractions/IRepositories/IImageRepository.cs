using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

/// <summary>
/// Interface for repository operations related to <see cref="Image"/> entities.
/// </summary>
public interface IImageRepository : IBaseRepository<Image>
{
    // This interface inherits all the methods from IBaseRepository<Image>
    // and can be extended with additional methods specific to the Image entity if needed.
}
