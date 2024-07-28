using VideoToPostGenerationAPI.DTOs.Incoming;
using VideoToPostGenerationAPI.DTOs.Outgoing;
using VideoToPostGenerationAPI.Services;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IPostService : IDisposable
{
    Task<PostResponse?> GetPostAsync(PostRequest post, string paltform);
}
