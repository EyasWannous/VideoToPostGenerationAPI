using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class GeneralPostRepository(AppDbContext context) : BaseRepository<GeneralPost>(context), IGeneralPostRepository
{
}
