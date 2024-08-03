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
    public YouTubeService()
    {
        _youtubeClient = new YoutubeClient();
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
}
