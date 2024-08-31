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


[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class VideoController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
    IWhisperService whisperService, IYouTubeService youTubeService,
    IGenerationService generationService, UserManager<User> userManager) : BaseController(unitOfWork, mapper)
{
    private readonly IFileService _fileService = fileService;
    private readonly IWhisperService _whisperService = whisperService;
    private readonly IYouTubeService _youTubeService = youTubeService;
    private readonly IGenerationService _generationService = generationService;
    private readonly UserManager<User> _userManager = userManager;

    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequestSizeLimit(FileSettings.MaxFileSizeInBytes)]
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
            return BadRequest("Could not make transcript from the audio, please try again");

        var titleRequest = new TitleRequest
        {
            Script = transcript.Text,
        };

        var title = await _generationService.GetTitleAsync(titleRequest);
        if (title is null)
            return BadRequest("Could not make title for this file, please try again");

        var audio = new Audio
        {
            Title = title.Title,
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
        audio.HasVideo = true;

        await _unitOfWork.Videos.AddAsync(video);
        await _unitOfWork.Audios.AddAsync(audio);

        await _unitOfWork.CompleteAsync();

        //result.Video = _mapper.Map<ResponseVideoDTO>(video);

        //return Ok(result);

        return CreatedAtAction(nameof(GetVideoById), new { id = video.Id }, _mapper.Map<ResponseAudioDTO>(audio));
    }

    [HttpGet("youtubeLink")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadYoutubeVideo([Required] string link = "https://www.youtube.com/watch?v=MYdKBuA3XF8")
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var filePath = await _fileService.DownloadFileAsync(FileSettings.VideosPath, link, false);
        if (filePath is null)
            return BadRequest("Could not donwload youtube video, please try again");

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
            return BadRequest("Could not make transcript from the audio, please try again");

        var title = await _youTubeService.GetVideoTitleAsync(link);
        if (title is null)
        {
            var titleRequest = new TitleRequest
            {
                Script = transcript,
            };

            var response = await _generationService.GetTitleAsync(titleRequest);
            title = response?.Title;
        }

        if (title is null)
            return BadRequest("Could not make title for this file, please try again");

        var audio = new Audio
        {
            Title = title,
            SizeBytes = await _fileService.GetFileSizeAsync(audioLink),
            AudioExtension = audioExtension.Split('.').Last() ?? AudioExtension.None.ToString(),
            Transcript = transcript,
            YoutubeLink = link,
            Link = audioLink,
            Duration = duration,
            UserId = loggedinUser!.Id,
            User = loggedinUser,
            HasVideo = true,
        };

        var thumbnailUrl = await _youTubeService.GetVideoThumbnailUrlAsync(link);
        if (thumbnailUrl is not null)
        {
            var audioThumbnail = new VideoThumbnail
            {
                Audio = audio,
                Link = thumbnailUrl,
                AudioId = audio.Id,
            };

            audio.VideoThumbnail = audioThumbnail;
            await _unitOfWork.VideoThumbnails.AddAsync(audioThumbnail);
        }

        var video = new Video
        {
            SizeBytes = await _fileService.GetFileSizeAsync(videoLink),
            Link = videoLink,
            VideoExtension = videoExtension.Split('.').Last() ?? VideoExtension.None.ToString(),
            Audio = audio,
            AudioId = audio.Id
        };

        audio.Video = video;
        audio.HasVideo = true;

        await _unitOfWork.Audios.AddAsync(audio);
        await _unitOfWork.Videos.AddAsync(video);

        await _unitOfWork.CompleteAsync();

        return Ok
        (
            new ResponseUploadFileDTO
            {
                Audio = _mapper.Map<ResponseAudioDTO>(audio),
                //Video = _mapper.Map<ResponseVideoDTO>(video),
                Link = videoLink,
                IsSuccess = true,
                Message = "Video Download Successfully",
            }
        );
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllVideos()
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var videos = await _unitOfWork.Videos.GetAllByUserIdAsync(loggedinUser!.Id);

        var mappedVideos = videos.Select(_mapper.Map<ResponseVideoDTO>).ToList();

        return Ok(mappedVideos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVideoById([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var video = await _unitOfWork.Videos.GetByIdAsync(id);
        if (video is null || video.Audio.User.Id != loggedinUser!.Id)
            return BadRequest("User Don't have access to this video");

        return Ok(_mapper.Map<ResponseVideoDTO>(video));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteVideo([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var video = await _unitOfWork.Videos.GetByIdAsync(id);
        if (video is null || video.Audio.User.Id != loggedinUser!.Id)
            return StatusCode(500, "Internal Server Error, User must be logged in");

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
