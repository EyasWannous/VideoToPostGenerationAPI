using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("{audioId:int}/{platform}")]
    public async Task<IActionResult> GetPosts([FromRoute] int audioId, [FromRoute] string platform)
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

        var result = await _postService.GetPostAsync(postReqiest, platform);
        if (result is null)
            return StatusCode(500, "Internal Server Error");

        var post = new Post
        {
            Description = result.Post,
            AudioId = audio.Id,
            Audio = audio,
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


        audio.Posts.Add(post);

        await _unitOfWork.CompleteAsync();

        return Ok(result);
    }

}
