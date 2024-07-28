
namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IYouTubeService
{
    Task<string?> GetVideoCaptions(string videoURL, string languagePrefix);
}
