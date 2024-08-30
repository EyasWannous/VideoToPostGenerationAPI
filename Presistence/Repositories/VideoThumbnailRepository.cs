using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class VideoThumbnailRepository(AppDbContext context) : BaseRepository<VideoThumbnail>(context),
    IVideoThumbnailRepository
{
}
