using VideoToPostGenerationAPI.DTOs.Incoming;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

/// <summary>
/// Defines the service for generating posts for various platforms.
/// </summary>
public interface IPostService : IDisposable
{
    /// <summary>
    /// Generates a post for the specified platform based on the provided post request.
    /// </summary>
    /// <param name="post">The post request containing details for generating the post.</param>
    /// <param name="platform">The platform for which the post is to be generated. E.g., "blog", "LinkedIn".</param>
    /// <returns>A task that represents the asynchronous operation, with a <see cref="PostResponse"/> result containing the details of the generated post, or null if the generation fails.</returns>
    /// <remarks>
    /// Example usage:
    /// 
    ///     var postResponse = await postService.GetPostAsync(postRequest, "blog");
    /// </remarks>
    Task<PostResponse?> GetPostAsync(PostRequest post, string platform);
}
