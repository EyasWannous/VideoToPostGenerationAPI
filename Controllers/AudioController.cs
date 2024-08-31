using AngleSharp.Html.Dom;
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
public class AudioController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
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
    //[AllowedExtensions()]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var result = await _fileService.StoreAsync<AudioExtension>(file, FileSettings.AudiosPath);
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        var extension = Path.GetExtension(result.Link);
        var duration = await _fileService.GetDurationAsync(result.Link);

        var transcript = await _whisperService.GetTranscriptAsync(result.Link);
        if (transcript is null)
            return BadRequest("Could not get transcript, please try again");

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
            SizeBytes = file.Length,
            AudioExtension = extension.Split('.').Last() ?? AudioExtension.None.ToString(),
            Transcript = transcript.Text,
            Link = result.Link,
            Duration = duration,
            UserId = loggedinUser!.Id,
            User = loggedinUser
        };

        await _unitOfWork.Audios.AddAsync(audio);

        await _unitOfWork.CompleteAsync();

        result.Audio = _mapper.Map<ResponseAudioDTO>(audio);

        return CreatedAtAction(nameof(GetAudioById), new { id = audio.Id }, _mapper.Map<ResponseAudioDTO>(audio));
    }

    [HttpGet("youtubeLink")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadYoutubeAudio([FromQuery][Required] string link)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var filePath = await _fileService.DownloadFileAsync(FileSettings.AudiosPath, link, true);
        if (filePath is null)
            return BadRequest("Could not download audio file, please check the link or try again");

        if (filePath.Contains("error"))
            return BadRequest(filePath);

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
            return BadRequest("Could not get transcript from youtube, please try again");

        var title = await _youTubeService.GetVideoTitleAsync(link);
        if (title is null)
            return BadRequest("Could not make title for this file, please try again");

        var audio = new Audio
        {
            Title = title,
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
            new ResponseUploadFileDTO
            {
                Audio = _mapper.Map<ResponseAudioDTO>(audio),
                Link = audioLink,
                IsSuccess = true,
                Message = "Video Download Successfully",
            }
        );
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAudios()
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audios = await _unitOfWork.Audios.GetAllByUserIdAsync(loggedinUser!.Id);

        var mappedAudios = audios.Select(_mapper.Map<ResponseAudioDTO>).ToList();

        return Ok(mappedAudios);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAudioById([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audio = await _unitOfWork.Audios.GetByIdAsync(id);
        if (audio is null || audio.User.Id != loggedinUser!.Id)
            return StatusCode(500, "Internal Server Error, User must be logged in");

        return Ok(_mapper.Map<ResponseAudioDTO>(audio));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAudio([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audio = await _unitOfWork.Audios.GetByIdToDeleteAsync(id);
        if (audio is null || audio.User.Id != loggedinUser!.Id)
            return StatusCode(500, "Internal Server Error, User must be logged in");

        var tasks = new List<Task>
        {
            _fileService.DeleteFileAsync(audio.Link),
            _unitOfWork.Audios.DeleteAsync(audio)
        };

        if (audio.Video is not null)
            tasks.Add(_fileService.DeleteFileAsync(audio.Video.Link));

        await Task.WhenAll(tasks);

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}
