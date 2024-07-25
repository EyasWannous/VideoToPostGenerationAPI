using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

namespace VideoToPostGenerationAPI.Domain.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IVideoRepository Videos { get; }
    IAudioRepository Audios { get; }
    IGeneralPostRepository GeneralPosts { get; }
    IPostRepository Posts { get; }
    IImageRepository Images { get; }
    Task<int> CompleteAsync();
}
