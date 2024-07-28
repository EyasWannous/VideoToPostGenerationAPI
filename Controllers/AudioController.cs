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
public class AudioController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
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

        var result = await _fileService.StoreAsync<AudioExtension>(file, FileSettings.AudiosPath); 
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        var extension = Path.GetExtension(result.Link);
        var duration = await _fileService.GetDurationAsync(result.Link);
        
        var audioTranscript = await _whisperService.GetTranscriptAsync(result.Link);
        if (audioTranscript is null)
            return BadRequest();

        var audio = new Audio
        {
            SizeBytes = file.Length,
            AudioExtension = extension.Split('.').Last() ?? AudioExtension.None.ToString(),
            Transcript = audioTranscript.Text,
            Link = result.Link,
            Duration = duration,
            UserId = loggedinUser!.Id,
            User = loggedinUser
        };

        await _unitOfWork.Audios.AddAsync(audio);

        await _unitOfWork.CompleteAsync();

        return Ok(result);
    }

    [HttpGet("youtubeLink")]
    public async Task<IActionResult> DownloadYoutubeAudio([FromQuery][Required] string link)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var filePath = await _fileService.DownloadFileAsync(FileSettings.AudiosPath, link, true);
        if (filePath is null)
            return BadRequest();

        var audioLink = $"{FileSettings.AudiosPath}{filePath}";

        var extension = Path.GetExtension(audioLink);
        var duration = await _fileService.GetDurationAsync(audioLink);

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
            AudioExtension = extension.Split('.').Last() ?? AudioExtension.None.ToString(),
            Transcript = transcript,
            YoutubeLink = link,
            Link = audioLink,
            Duration = duration,
            UserId = loggedinUser!.Id,
            User = loggedinUser
        };

        await _unitOfWork.Audios.AddAsync(audio);

        await _unitOfWork.CompleteAsync();

        return Ok
        (
            new ResponseUploadFile
            {
                Link = audioLink,
                IsSuccess = true,
                Message = "Video Download Successfully",
            }
        );
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAudios()
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);
        
        var audios = await _unitOfWork.Audios.GetAllByUserIdAsync(loggedinUser!.Id);

        var mappedAudios = audios.Select(_mapper.Map<ResponseAudio>).ToList();

        return Ok(mappedAudios);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetAudioById([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);
        
        var audio = await _unitOfWork.Audios.GetByIdAsync(id);
        if (audio is null || audio.User.Id != loggedinUser!.Id)
            return BadRequest();

        var mappedAudio = _mapper.Map<ResponseAudio>(audio);
        if (mappedAudio is null)
            return BadRequest();

        return Ok(mappedAudio);
    }
}
