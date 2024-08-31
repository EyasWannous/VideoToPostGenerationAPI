using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class HeaderRepository : BaseRepository<Header>, IHeaderRepository
{
    public HeaderRepository(AppDbContext context) : base(context)
    {
    }
}
