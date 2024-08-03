namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

/// <summary>
/// Provides capabilities to interact with YouTube for retrieving video captions.
/// </summary>
public interface IYouTubeService
{
    /// <summary>
    /// Retrieves the captions for a specified YouTube video in the given language.
    /// </summary>
    /// <param name="videoURL">The URL of the YouTube video from which captions are to be retrieved.</param>
    /// <param name="languagePrefix">The language prefix for the desired captions (e.g., "en" for English). This is used to filter captions in the specified language.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a string with concatenated captions if available, or null if captions are not available in the specified language.</returns>
    /// <remarks>
    /// This method fetches the closed captions from the YouTube video and returns them as a single concatenated string.
    /// 
    /// Sample usage:
    /// 
    ///     var captions = await youTubeService.GetVideoCaptions("https://www.youtube.com/watch?v=example", "en");
    /// </remarks>
    Task<string?> GetVideoCaptions(string videoURL, string languagePrefix);
}
