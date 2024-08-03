using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Domain.Enums;
using VideoToPostGenerationAPI.DTOs.Outgoing;
using YoutubeExplode.Videos;

namespace VideoToPostGenerationAPI.Controllers;

/// <summary>
/// Controller for handling post-related operations.
/// </summary>
[Authorize]
public class PostController(IUnitOfWork unitOfWork, IMapper mapper,
    IPostService postService, UserManager<User> userManager) : BaseController(unitOfWork, mapper)
{
    private readonly IPostService _postService = postService;
    private readonly UserManager<User> _userManager = userManager;

    /// <summary>
    /// Generates a new post for a video on a specified platform.
    /// </summary>
    /// <param name="audioId">The ID of the video.</param>
    /// <param name="platform">The platform for which to generate the post.</param>
    /// <returns>A newly created post related to the specified video and platform.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /{videoId:int}?platform=Blog
    ///
    /// </remarks>
    /// <response code="200">Returns the newly created post.</response>
    /// <response code="400">If the video does not exist or the user is not authorized.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpGet("{audioId:int}")]
    public async Task<IActionResult> GetPosts([FromRoute] int audioId, [Required][FromQuery] string platform)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audio = await _unitOfWork.Audios.GetByIdAsync(audioId);
        if (audio is null || audio.UserId != loggedinUser!.Id)
            return BadRequest();

        var postReqiest = new PostRequest
        {
            Link = audio.YoutubeLink ?? "No Link",
            Script = audio.Transcript,
        };

        var postResponse = await _postService.GetPostAsync(postReqiest, platform);
        if (postResponse is null)
            return StatusCode(500, "Internal Server Error");

        var post = new Post
        {
            Description = postResponse.Post,
            Platform = platform,
            AudioId = audio.Id,
            Audio = audio,
        };

        if (platform.Equals(Platform.Blog.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            var header = new Header
            {
                Title = postResponse.Title,
                Post = post,
                PostId = post.Id
            };

            post.Header = header;
        }

        audio.Posts.Add(post);

        await _unitOfWork.CompleteAsync();

        return Ok(_mapper.Map<ResponsePostDTO>(post));
    }

    /// <summary>
    /// Retrieves old posts for a video.
    /// </summary>
    /// <param name="audioId">The ID of the video.</param>
    /// <returns>A list of old posts related to the specified video.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /old/{videoId:int}
    ///
    /// </remarks>
    /// <response code="200">Returns a list of old posts.</response>
    /// <response code="400">If the video does not exist or the user is not authorized.</response>
    [HttpGet("old/{audioId:int}")]
    public async Task<IActionResult> GetOldPosts([FromRoute] int audioId)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audio = await _unitOfWork.Audios.GetByIdAsync(audioId);
        if (audio is null || audio.UserId != loggedinUser!.Id)
            return BadRequest();

        var posts = await _unitOfWork.Posts.GetAllByAudioIdAsync(audio.Id);

        var result = posts.Select(_mapper.Map<ResponsePostDTO>).ToList();

        return Ok(result);
    }
}
