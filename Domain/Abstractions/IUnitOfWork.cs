using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

namespace VideoToPostGenerationAPI.Domain.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }

    IAudioRepository Audios { get; }

    IVideoRepository Videos { get; }

    IHeaderRepository Headers { get; }

    IPostRepository Posts { get; }

    IPostImageRepository PostsImages { get; }

    IPostOptionsRepository PostsOptions { get; }

    IVideoThumbnailRepository VideoThumbnails { get; }

    Task<int> CompleteAsync();
}
