using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

/// <summary>
/// Interface for user repository operations.
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    // Add any user-specific repository methods here if needed
}
