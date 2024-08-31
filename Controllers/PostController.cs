using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Domain.Enums;
using VideoToPostGenerationAPI.Domain.Settings;
using VideoToPostGenerationAPI.DTOs.Incoming;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Controllers;

[Authorize]
public class PostController : BaseController
{
    private readonly IGenerationService _generationService;
    private readonly UserManager<User> _userManager;
    private readonly IFileService _fileService;

    public PostController(IUnitOfWork unitOfWork, IMapper mapper, IGenerationService postService, UserManager<User> userManager, IFileService fileService)
        : base(unitOfWork, mapper)
    {
        _generationService = postService;
        _userManager = userManager;
        _fileService = fileService;
    }

    [HttpPost("{audioId:int}")]
    public async Task<IActionResult> GetPosts([FromRoute] int audioId, [FromBody] RequestPostDTO requestPostDTO)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audio = await _unitOfWork.Audios.GetAudioByIdForPost(audioId);
        if (audio is null || audio.UserId != loggedinUser!.Id)
            return NotFound();

        // Convert the input string to lowercase to compare case-insensitively
        string platformString = requestPostDTO.Platform.ToLower();

        //bool isValidPlatform = Enum.TryParse<Platform>(platformString, true, out _);

        //if (!isValidPlatform)
        //    return BadRequest($"{platformString} is not a Platform");

        var postRequest = new PostRequest
        {
            Link = audio.YoutubeLink,
            Script = audio.Transcript,
            VideoLink = audio.Video?.Link,
            PostOptionsRequest = new PostOptionsRequest
            {
                Platform = platformString,
                AdditionalPrompt = requestPostDTO.AdditionalPrompt,
                PointOfView = requestPostDTO.PointOfView == 0 ? PointOfView.Auto.ToString() : requestPostDTO.PointOfView.ToString(),
                PostFormat = requestPostDTO.PostFormat == 0 ? PostFormat.Auto.ToString() : requestPostDTO.PostFormat.ToString().ToLower(),
                //PrimaryKeyPhrase = requestPostDTO.PrimaryKeyPhrase,
                UseEmojis = requestPostDTO.UseEmojis,
                WordCount = requestPostDTO.WordCount
            },
        };

        var postOptions = new PostOptions
        {
            Platform = platformString,
            PointOfView = requestPostDTO.PointOfView,
            //PrimaryKeyPhrase = requestPostDTO.PrimaryKeyPhrase ?? "",
            PostFormat = requestPostDTO.PostFormat,
            UseEmojis = requestPostDTO.UseEmojis,
            AdditionalPrompt = requestPostDTO.AdditionalPrompt ?? "",
        };


        var postResponse = await _generationService.GetPostAsync(postRequest);
        if (postResponse is null)
            return StatusCode(500, "Internal Server Error");

        //var tasks = new List<Task<string>>();

        //foreach (var imageUrl in postResponse.Images)
        //{
        //    tasks.Add(_fileService.DownloadImageAsync(imageUrl));
        //    //images.Add(await _fileService.DownloadImageAsync(imageUrl));
        //}

        var post = new Post
        {
            Description = postResponse.Post,
            PostOptions = postOptions,
            Rate = postResponse.Rate,
            AudioId = audio.Id,
            Audio = audio,
        };

        postOptions.Post = post;

        if (requestPostDTO.Platform.Equals(Platform.Blog.ToString().ToLower()))
        {
            var header = new Header
            {
                Title = postResponse.Title,
                Post = post,
                PostId = post.Id
            };

            post.Header = header;
            await _unitOfWork.Headers.AddAsync(header);
        }


        //var images = await Task.WhenAll<string>(tasks);

        //foreach (var image in images)
        //{
        //    var imageLink = $"{FileSettings.ImagesPath}{image}";

        //    var imageExtension = Path.GetExtension(imageLink);

        //    var postImage = new PostImage
        //    {
        //        Link = imageLink,
        //        SizeBytes = await _fileService.GetFileSizeAsync(imageLink),
        //        ImageExtension = imageExtension.Split('.').Last() ?? ImageExtension.None.ToString(),
        //        Post = post,
        //    };

        //    post.PostImages.Add(postImage);
        //    await _unitOfWork.PostsImages.AddAsync(postImage);
        //}

        audio.Posts.Add(post);

        await _unitOfWork.PostsOptions.AddAsync(postOptions);
        await _unitOfWork.Posts.AddAsync(post);

        await _unitOfWork.CompleteAsync();

        await DeleteImages(postResponse.Images);

        return Ok(_mapper.Map<ResponsePostDTO>(post));
    }

    [HttpGet("old/{audioId:int}")]
    public async Task<IActionResult> GetOldPosts([FromRoute] int audioId)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audio = await _unitOfWork.Audios.GetByIdAsync(audioId);
        if (audio is null || audio.UserId != loggedinUser!.Id)
            return StatusCode(500, "Internal Server Error, User must be logged in");

        var posts = await _unitOfWork.Posts.GetAllByAudioIdAsync(audio.Id);

        var result = posts.Select(_mapper.Map<ResponsePostDTO>).ToList();

        return Ok(result);
    }

    [HttpGet("ws/{audioId:int}")]
    public async Task<IActionResult> GetPostWS([FromRoute] int audioId, [FromBody] RequestPostDTO requestPostDTO)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audio = await _unitOfWork.Audios.GetByIdAsync(audioId);
        if (audio is null || audio.UserId != loggedinUser!.Id)
            return StatusCode(500, "Internal Server Error, User must be logged in");


        var postRequest = new PostRequest
        {
            Link = audio.YoutubeLink,
            Script = audio.Transcript,
            VideoLink = audio.Video?.Link,
            PostOptionsRequest = new PostOptionsRequest
            {
                Platform = requestPostDTO.Platform,
                AdditionalPrompt = requestPostDTO.AdditionalPrompt,
                PointOfView = requestPostDTO.PointOfView.ToString().ToLower(),
                PostFormat = requestPostDTO.PostFormat.ToString().ToLower(),
                //PrimaryKeyPhrase = requestPostDTO.PrimaryKeyPhrase,
                UseEmojis = requestPostDTO.UseEmojis,
                WordCount = requestPostDTO.WordCount
            },
        };

        var postsResponse = await _generationService.GetPostWSAsync(loggedinUser!.Id.ToString(), postRequest, requestPostDTO.Platform);
        if (postsResponse.IsNullOrEmpty())
            return StatusCode(500, "Internal Server Error");

        var postOptions = new PostOptions
        {
            Platform = requestPostDTO.Platform,
            PointOfView = requestPostDTO.PointOfView,
            //PrimaryKeyPhrase = requestPostDTO.PrimaryKeyPhrase,
            PostFormat = requestPostDTO.PostFormat,
            UseEmojis = requestPostDTO.UseEmojis,
            AdditionalPrompt = requestPostDTO.AdditionalPrompt ?? "",
        };

        foreach (var postResponse in postsResponse)
        {
            if (postResponse is null)
                continue;

            var post = new Post
            {
                Description = postResponse.Post,
                PostOptions = postOptions,
                AudioId = audio.Id,
                Audio = audio,
            };

            postOptions.Post = post;


            if (requestPostDTO.Platform.Equals(Platform.Blog.ToString().ToLower()))
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
        }

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }


    private async Task DeleteImages(List<string> images)
    {
        //var deleteTasks = new List<Task<bool>>();

        foreach (var imageUrl in images)
        {
            //deleteTasks.Add(_generationService.DeleteImageAsync(imageUrl.Split("/").Last()));
            await _generationService.DeleteImageAsync(imageUrl.Split("/").Last());
        }

        //await Task.WhenAll<bool>(deleteTasks);
    }


    //[HttpPost("image/{audioId:int}/{postId:int}")]
    //public async Task<IActionResult> GetPostsImages([FromRoute] int audioId, [FromRoute] int postId, [FromBody] RequestPostDTO requestPostDTO)
    //{
    //    var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

    //    var audio = await _unitOfWork.Audios.GetAudioByIdForPost(audioId);
    //    if (audio is null || audio.UserId != loggedinUser!.Id)
    //        return NotFound();

    //    if (audio.Video is null)
    //        return BadRequest("This audio file dose not have video");

    //    var post = await _unitOfWork.Posts.GetByPostIdAsync(postId);
    //    if (post is null || post.AudioId != audioId)
    //        return NotFound();

    //    // Convert the input string to lowercase to compare case-insensitively
    //    string platformString = requestPostDTO.Platform.ToLower();

    //    // Try to parse the string to the enum
    //    bool isValidPlatform = Enum.TryParse<Platform>(platformString, true, out _);

    //    if (!isValidPlatform)
    //        return BadRequest($"{platformString} is not a Platform");

    //    var postRequest = new PostRequest
    //    {
    //        Link = audio.YoutubeLink,
    //        Script = audio.Transcript,
    //        VideoLink = audio.Video?.Link,
    //        PostOptionsRequest = new PostOptionsRequest
    //        {
    //            Platform = platformString,
    //            AdditionalPrompt = requestPostDTO.AdditionalPrompt,
    //            PointOfView = requestPostDTO.PointOfView.ToString(),
    //            PostFormat = requestPostDTO.PostFormat.ToString(),
    //            //PrimaryKeyPhrase = requestPostDTO.PrimaryKeyPhrase,
    //            UseEmojis = requestPostDTO.UseEmojis,
    //            WordCount = requestPostDTO.WordCount
    //        },
    //    };

    //    var imagesResponse = await _generationService.GetImagesForPost(postRequest);
    //    if (imagesResponse is null)
    //        return StatusCode(500, "Internal Server Error");

    //    var tasks = new List<Task<string>>();
    //    //var images = new List<string>();

    //    foreach (var imageUrl in imagesResponse)
    //    {
    //        tasks.Add(_fileService.DownloadImageAsync(imageUrl));
    //        //images.Add(await _fileService.DownloadImageAsync(imageUrl));
    //    }

    //    var images = await Task.WhenAll<string>(tasks);

    //    foreach (var image in images)
    //    {
    //        var imageLink = $"{FileSettings.ImagesPath}{image}";

    //        var imageExtension = Path.GetExtension(imageLink);

    //        var postImage = new PostImage
    //        {
    //            Link = imageLink,
    //            SizeBytes = await _fileService.GetFileSizeAsync(imageLink),
    //            ImageExtension = imageExtension.Split('.').Last() ?? ImageExtension.None.ToString(),
    //            Post = post,
    //            PostId = post.Id,
    //        };

    //        post.PostImages.Add(postImage);
    //        await _unitOfWork.PostsImages.AddAsync(postImage);
    //    }


    //    await _unitOfWork.CompleteAsync();

    //    await DeleteImages(imagesResponse);

    //    return Ok(_mapper.Map<ResponsePostDTO>(post));
    //}
}
