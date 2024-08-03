using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Domain.Enums;
using VideoToPostGenerationAPI.Domain.Settings;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Controllers;


/// <summary>
/// Controller for handling video-related operations.
/// </summary>
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class VideoController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
    IWhisperService whisperService, IYouTubeService youTubeService, UserManager<User> userManager) : BaseController(unitOfWork, mapper)
{
    private readonly IFileService _fileService = fileService;
    private readonly IWhisperService _whisperService = whisperService;
    private readonly IYouTubeService _youTubeService = youTubeService;
    private readonly UserManager<User> _userManager = userManager;

    /// <summary>
    /// Uploads a video file.
    /// </summary>
    /// <param name="file">The video file to upload.</param>
    /// <returns>A newly created Video object related to the uploaded file.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /upload
    ///     {
    ///        "file": "video.mp4"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created item.</response>
    /// <response code="400">If the item is null.</response>
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload([Required] IFormFile file)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var result = await _fileService.StoreAsync<VideoExtension>(file, FileSettings.VideosPath);
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        var videoExtension = Path.GetExtension(result.Link);
        var duration = await _fileService.GetDurationAsync(result.Link);

        var audioFullPath = await _fileService.ConvertVideoToAudioAsync(result.Link);
        var audioExtension = Path.GetExtension(audioFullPath);
        var audioLink = $"{FileSettings.AudiosPath}{audioFullPath.Split('\\').Last()}";

        var transcript = await _whisperService.GetTranscriptAsync(audioLink);
        if (transcript is null)
            return BadRequest();

        var audio = new Audio
        {
            SizeBytes = await _fileService.GetFileSizeAsync(audioLink),
            AudioExtension = audioExtension.Split('.').Last() ?? AudioExtension.None.ToString(),
            Transcript = transcript.Text,
            Link = audioLink,
            Duration = duration,
            UserId = loggedinUser!.Id,
            User = loggedinUser
        };

        var video = new Video
        {
            SizeBytes = file.Length,
            Link = result.Link,
            VideoExtension = videoExtension.Split('.').Last() ?? VideoExtension.None.ToString(),
            Audio = audio,
            AudioId = audio.Id,
        };

        audio.Video = video;

        await _unitOfWork.Videos.AddAsync(video);
        await _unitOfWork.Audios.AddAsync(audio);

        await _unitOfWork.CompleteAsync();

        //result.Video = _mapper.Map<ResponseVideoDTO>(video);

        //return Ok(result);

        return CreatedAtAction(nameof(GetVideoById), new { id = video.Id }, _mapper.Map<ResponseVideoDTO>(video));
    }

    /// <summary>
    /// Downloads a YouTube video and processes it.
    /// </summary>
    /// <param name="link">The YouTube video link.</param>
    /// <returns>Information about the downloaded video.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /youtubeLink?link=https://www.youtube.com/watch?v=MYdKBuA3XF8
    ///
    /// </remarks>
    /// <response code="200">Returns the information about the downloaded video.</response>
    /// <response code="400">If the download or processing fails.</response>
    [HttpGet("youtubeLink")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadYoutubeVideo([Required] string link = "https://www.youtube.com/watch?v=MYdKBuA3XF8")
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var filePath = await _fileService.DownloadFileAsync(FileSettings.VideosPath, link, false);
        if (filePath is null)
            return BadRequest();

        var videoLink = $"{FileSettings.VideosPath}{filePath}";

        var videoExtension = Path.GetExtension(videoLink);
        var duration = await _fileService.GetDurationAsync(videoLink);

        var audioFullPath = await _fileService.ConvertVideoToAudioAsync(videoLink);
        var audioExtension = Path.GetExtension(audioFullPath);
        var audioLink = $"{FileSettings.AudiosPath}{audioFullPath.Split('\\').Last()}";

        var transcript = await _youTubeService.GetVideoCaptions(link, "en");
        if (transcript is null)
        {
            var transcriptionResponse = await _whisperService.GetTranscriptAsync(audioLink);

            transcript = transcriptionResponse?.Text;
        }

        if (transcript is null)
            return BadRequest();

        var audio = new Audio
        {
            SizeBytes = await _fileService.GetFileSizeAsync(audioLink),
            AudioExtension = audioExtension.Split('.').Last() ?? AudioExtension.None.ToString(),
            Transcript = transcript,
            YoutubeLink = link,
            Link = audioLink,
            Duration = duration,
            UserId = loggedinUser!.Id,
            User = loggedinUser
        };

        var video = new Video
        {
            SizeBytes = await _fileService.GetFileSizeAsync(videoLink),
            Link = videoLink,
            VideoExtension = videoExtension.Split('.').Last() ?? VideoExtension.None.ToString(),
            Audio = audio,
            AudioId = audio.Id,
        };

        audio.Video = video;

        await _unitOfWork.Audios.AddAsync(audio);
        await _unitOfWork.Videos.AddAsync(video);

        await _unitOfWork.CompleteAsync();

        return Ok
        (
            new ResponseUploadFileDTO
            {
                //Audio = _mapper.Map<ResponseAudioDTO>(audio),
                Video = _mapper.Map<ResponseVideoDTO>(video),
                Link = videoLink,
                IsSuccess = true,
                Message = "Video Download Successfully",
            }
        );
    }

    /// <summary>
    /// Retrieves all videos uploaded by the logged-in user.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /all
    ///
    /// </remarks>
    /// <returns>A list of videos uploaded by the logged-in user.</returns>
    /// <response code="200">Returns the list of videos.</response>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllVideos()
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var videos = await _unitOfWork.Videos.GetAllByUserIdAsync(loggedinUser!.Id);

        var mappedVideos = videos.Select(_mapper.Map<ResponseVideoDTO>).ToList();

        return Ok(mappedVideos);
    }

    /// <summary>
    /// Retrieves a specific video by its ID.
    /// </summary>
    /// <param name="id">The ID of the video.</param>
    /// <returns>The video with the specified ID.</returns>
    /// <response code="200">Returns the video with the specified ID.</response>
    /// <response code="400">If the video is not found or the user is not authorized to view it.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVideoById([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var video = await _unitOfWork.Videos.GetByIdAsync(id);
        if (video is null || video.Audio.User.Id != loggedinUser!.Id)
            return BadRequest();

        var mappedVideo = _mapper.Map<ResponseVideoDTO>(video);
        if (mappedVideo is null)
            return BadRequest();

        return Ok(mappedVideo);
    }

    /// <summary>
    /// Deletes a specific video by its ID.
    /// </summary>
    /// <param name="id">The ID of the video to delete.</param>
    /// <returns>No content if the video is successfully deleted.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /{id}
    ///
    /// </remarks>
    /// <response code="204">If the video is successfully deleted.</response>
    /// <response code="400">If the video is not found or the user is not authorized to delete it.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteVideo([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var video = await _unitOfWork.Videos.GetByIdAsync(id);
        if (video is null || video.Audio.User.Id != loggedinUser!.Id)
            return BadRequest();

        var tasks = new List<Task>
        {
            _fileService.DeleteFileAsync(video.Link),
            _fileService.DeleteFileAsync(video.Audio.Link),
            _unitOfWork.Audios.DeleteAsync(video.Audio),
        };

        await Task.WhenAll(tasks);

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }


    //[HttpGet("Test")]
    //public async Task<IActionResult> Test()
    //{
    //    //var principal = HttpContext.User;

    //    //return SignIn(principal);

    //    return Accepted();
    //}
}
