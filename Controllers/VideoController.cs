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
        
        var audioTranscript = await _whisperService.GetTranscriptAsync(audioLink);
        if (audioTranscript is null)
            return BadRequest();

        var audio = new Audio
        {
            SizeBytes = await _fileService.GetFileSizeAsync(audioLink),
            AudioExtension = audioExtension.Split('.').Last() ?? AudioExtension.None.ToString(),
            Transcript = audioTranscript.Text,
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

        await _unitOfWork.Audios.AddAsync(audio);
        await _unitOfWork.Videos.AddAsync(video);

        await _unitOfWork.CompleteAsync();

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

        var audioTranscript = await _youTubeService.GetVideoCaptions(link, "en");
        if (audioTranscript is null)
        {
            var transcriptionResponse = await _whisperService.GetTranscriptAsync(audioLink);

            audioTranscript = transcriptionResponse?.Text;
        }

        if (audioTranscript is null)
            return BadRequest();

        var audio = new Audio
        {
            SizeBytes = await _fileService.GetFileSizeAsync(audioLink),
            AudioExtension = audioExtension.Split('.').Last() ?? AudioExtension.None.ToString(),
            Transcript = audioTranscript,
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
            new ResponseUploadFile
            {
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

        var mappedVideos = videos.Select(_mapper.Map<ResponseVideo>).ToList();

        return Ok(mappedVideos);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetVideoById([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var video = await _unitOfWork.Videos.GetByIdAsync(id);
        if (video is null || video.Audio.User.Id != loggedinUser!.Id)
            return BadRequest();

        var mappedVideo = _mapper.Map<ResponseVideo>(video);
        if (mappedVideo is null)
            return BadRequest();

        return Ok(mappedVideo);
    }
}
