using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class PostImageRepository : BaseRepository<PostImage>, IPostImageRepository
{
    public PostImageRepository(AppDbContext context) : base(context)
    {
    }
}
