using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

/// <summary>
/// Repository for handling operations related to <see cref="Header"/> entities.
/// </summary>
public class HeaderRepository : BaseRepository<Header>, IHeaderRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public HeaderRepository(AppDbContext context) : base(context)
    {
    }
}
