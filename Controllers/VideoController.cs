using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Domain.Enums;
using VideoToPostGenerationAPI.Domain.Settings;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Controllers;

[Authorize]
public class VideoController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
    IWhisperService whisperService, IYouTubeService youTubeService, UserManager<User> userManager) : BaseController(unitOfWork, mapper)
{
    private readonly IFileService _fileService = fileService;
    private readonly IWhisperService _whisperService = whisperService;
    private readonly IYouTubeService _youTubeService = youTubeService;
    private readonly UserManager<User> _userManager = userManager;

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
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

        var video = new Video
        {
            SizeBytes = file.Length,
            VideoExtension = videoExtension.Split('.').Last() ?? VideoExtension.None.ToString(),
            Transcript = transcript.Text,
            Link = result.Link,
            Duration = duration,
            UserId = loggedinUser!.Id,
            User = loggedinUser
        };

        var audio = new Audio
        {
            SizeBytes = await _fileService.GetFileSizeAsync(audioLink),
            Link = audioLink,
            AudioExtension = audioExtension.Split('.').Last() ?? AudioExtension.None.ToString(),
            Video = video,
            VideoId = video.Id,
        };

        video.Audio = audio;

        await _unitOfWork.Audios.AddAsync(audio);
        await _unitOfWork.Videos.AddAsync(video);

        await _unitOfWork.CompleteAsync();

        result.Video = _mapper.Map<ResponseVideoDTO>(video);

        return Ok(result);
    }


    [HttpGet("youtubeLink")]
    public async Task<IActionResult> DownloadYoutubeVideo(string link)
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

        var video = new Video
        {
            SizeBytes = await _fileService.GetFileSizeAsync(videoLink),
            VideoExtension = videoExtension.Split('.').Last() ?? VideoExtension.None.ToString(),
            Transcript = transcript,
            YoutubeLink = link,
            Link = videoLink,
            Duration = duration,
            UserId = loggedinUser!.Id,
            User = loggedinUser
        };

        var audio = new Audio
        {
            SizeBytes = await _fileService.GetFileSizeAsync(audioLink),
            Link = audioLink,
            AudioExtension = audioExtension.Split('.').Last() ?? AudioExtension.None.ToString(),
            Video = video,
            VideoId = video.Id,
        };

        video.Audio = audio;

        await _unitOfWork.Audios.AddAsync(audio);
        await _unitOfWork.Videos.AddAsync(video);

        await _unitOfWork.CompleteAsync();

        return Ok
        (
            new ResponseUploadFileDTO
            {
                Video = _mapper.Map<ResponseVideoDTO>(video),
                Link = videoLink,
                IsSuccess = true,
                Message = "Video Download Successfully",
            }
        );
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllVideos()
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var videos = await _unitOfWork.Videos.GetAllByUserIdAsync(loggedinUser!.Id);

        var mappedVideos = videos.Select(_mapper.Map<ResponseVideoDTO>).ToList();

        return Ok(mappedVideos);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetVideoById([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var video = await _unitOfWork.Videos.GetByIdAsync(id);
        if (video is null || video.User.Id != loggedinUser!.Id)
            return BadRequest();

        var mappedVideo = _mapper.Map<ResponseVideoDTO>(video);
        if (mappedVideo is null)
            return BadRequest();

        return Ok(mappedVideo);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteVideo([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var video = await _unitOfWork.Videos.GetByIdAsync(id);
        if (video is null || video.User.Id != loggedinUser!.Id)
            return BadRequest();

        var tasks = new List<Task>
        {
            _fileService.DeleteFileAsync(video.Link),
            _unitOfWork.Videos.DeleteAsync(video),
        };

        await Task.WhenAll(tasks);

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}
