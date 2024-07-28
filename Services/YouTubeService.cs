using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using YoutubeExplode;

namespace VideoToPostGenerationAPI.Services;

public class YouTubeService : IYouTubeService
{
    private readonly YoutubeClient _youtubeClient = new();

    public async Task<string?> GetVideoCaptions(string videoURL, string languagePrefix)
    {
        var trackManifest = await _youtubeClient.Videos.ClosedCaptions.GetManifestAsync(videoURL);
        var trackInfo = trackManifest.TryGetByLanguage(languagePrefix); // Get English captions
        if (trackInfo is null)
            return null;
        
        var track = await _youtubeClient.Videos.ClosedCaptions.GetAsync(trackInfo);
        return string.Join(" ", track.Captions.Select(caption => caption.Text));
    }
}
