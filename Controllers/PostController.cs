using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using static VideoToPostGenerationAPI.Services.PostService;

namespace VideoToPostGenerationAPI.Controllers;

public class PostController(IUnitOfWork unitOfWork, IMapper mapper, IPostService postService) : BaseController(unitOfWork, mapper)
{
    private readonly IPostService _postService = postService;

    [HttpPost("posts")]
    public async Task<IActionResult> GetPosts([FromBody] ScoringItem item)
    {
        //var result = await _postService.GetPostAsync();
        //var result2 = await _postService.GetSomething("issa", 15); 
        var result = await _postService.PostScoringItemAsync(item);

        return Ok(result);
    }
}
