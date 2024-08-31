using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using YoutubeExplode;

namespace VideoToPostGenerationAPI.Services;

public class YouTubeService : IYouTubeService
{
    private readonly YoutubeClient _youtubeClient;

    public YouTubeService(HttpClient client)
    {
        client.Timeout = TimeSpan.FromMinutes(30);
        _youtubeClient = new YoutubeClient(http: client);
    }

    public async Task<string?> GetVideoCaptions(string videoURL, string languagePrefix)
    {
        try
        {
            var trackManifest = await _youtubeClient.Videos.ClosedCaptions.GetManifestAsync(videoURL);
            var trackInfo = trackManifest.TryGetByLanguage(languagePrefix);

            if (trackInfo == null)
            {
                return null;
            }

            var track = await _youtubeClient.Videos.ClosedCaptions.GetAsync(trackInfo);
            var captions = track.Captions.Select(c => c.Text).ToList();

            return string.Join(" ", captions);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            // For example: _logger.LogError(ex, "An error occurred while retrieving YouTube captions.");
            throw new ApplicationException("An error occurred while retrieving video captions.", ex);
        }
    }

    public async Task<string> GetVideoTitleAsync(string videoURL)
    {
        try
        {
            var video = await _youtubeClient.Videos.GetAsync(videoURL);
            return video.Title;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving the video title.", ex);
        }
    }

    public async Task<string?> GetVideoThumbnailUrlAsync(string videoURL)
    {
        try
        {
            var video = await _youtubeClient.Videos.GetAsync(videoURL);
            var thumbnailUrl = video.Thumbnails.MaxBy(t => t.Resolution.Width)?.Url;
            return thumbnailUrl;
        }
        catch (Exception ex)
        {
            return null;
            // Log the exception or handle it as needed
            //throw new ApplicationException("An error occurred while retrieving the video thumbnail.", ex);
        }
    }
}
