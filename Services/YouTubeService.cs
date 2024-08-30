using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using YoutubeExplode;

namespace VideoToPostGenerationAPI.Services;

/// <summary>
/// Service for interacting with YouTube to retrieve video captions.
/// </summary>
public class YouTubeService : IYouTubeService
{
    private readonly YoutubeClient _youtubeClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="YouTubeService"/> class.
    /// </summary>
    public YouTubeService(HttpClient client)
    {
        client.Timeout = TimeSpan.FromMinutes(30);
        _youtubeClient = new YoutubeClient(http: client);
    }

    /// <summary>
    /// Retrieves the captions for a YouTube video in the specified language.
    /// </summary>
    /// <param name="videoURL">The URL of the YouTube video.</param>
    /// <param name="languagePrefix">The language prefix for the desired captions (e.g., "en" for English).</param>
    /// <returns>The concatenated captions as a single string, or null if captions are not available in the specified language.</returns>
    /// <remarks>
    /// Sample usage:
    ///
    ///     var captions = await youTubeService.GetVideoCaptions("https://www.youtube.com/watch?v=example", "en");
    ///
    /// </remarks>
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

    /// <summary>
    /// Retrieves the title of a YouTube video.
    /// </summary>
    /// <param name="videoURL">The URL of the YouTube video.</param>
    /// <returns>The title of the video as a string.</returns>
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

    /// <summary>
    /// Retrieves the thumbnail URL of a YouTube video.
    /// </summary>
    /// <param name="videoURL">The URL of the YouTube video.</param>
    /// <returns>The URL of the video thumbnail.</returns>
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
