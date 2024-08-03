using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

/// <summary>
/// Repository for handling operations related to <see cref="User"/> entities.
/// </summary>
public class UserRepository : BaseRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    // Additional user-specific methods can be added here
}
