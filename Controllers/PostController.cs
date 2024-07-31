using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Domain.Enums;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Controllers;

[Authorize]
public class PostController(IUnitOfWork unitOfWork, IMapper mapper,
    IPostService postService, UserManager<User> userManager) : BaseController(unitOfWork, mapper)
{
    private readonly IPostService _postService = postService;
    private readonly UserManager<User> _userManager = userManager;

    [HttpGet("{videoId:int}")]
    public async Task<IActionResult> GetPosts([FromRoute] int videoId, [Required][FromQuery] string platform)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var video = await _unitOfWork.Videos.GetByIdAsync(videoId);
        if (video is null || video.UserId != loggedinUser!.Id)
            return BadRequest();

        var postReqiest = new PostRequest
        {
            Link = video.YoutubeLink ?? "No Link",
            Script = video.Transcript,
        };

        var result = await _postService.GetPostAsync(postReqiest, platform);
        if (result is null)
            return StatusCode(500, "Internal Server Error");

        var post = new Post
        {
            Description = result.Post,
            VideoId = video.Id,
            Video = video,
        };

        if (platform.Equals(Platform.Blog.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            var header = new Header
            {
                Title = result.Title,
                Post = post,
                PostId = post.Id
            };

            post.Header = header;
        }


        video.Posts.Add(post);

        await _unitOfWork.CompleteAsync();

        return Ok(result);
    }

    [HttpGet("old/{videoId:int}")]
    public async Task<IActionResult> GetOldPosts([FromRoute] int videoId)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var video = await _unitOfWork.Videos.GetByIdAsync(videoId);
        if (video is null || video.UserId != loggedinUser!.Id)
            return BadRequest();

        var posts = await _unitOfWork.Posts.GetAllByVideoIdAsync(video.Id);

        var result = posts.Select(_mapper.Map<ResponsePost>).ToList();

        return Ok(posts);
    }
}
