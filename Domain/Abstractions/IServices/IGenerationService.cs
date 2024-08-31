using VideoToPostGenerationAPI.DTOs.Incoming;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IGenerationService : IDisposable
{
    Task<bool> DeleteImageAsync(string imageName);
    Task<List<string>?> GetImagesForPost(PostRequest post);

    Task<PostResponse?> GetPostAsync(PostRequest post);

    Task<List<PostResponse?>> GetPostWSAsync(string userId, PostRequest post, string platform);

    Task<TitleResponse?> GetTitleAsync(TitleRequest titleRequest);
}
