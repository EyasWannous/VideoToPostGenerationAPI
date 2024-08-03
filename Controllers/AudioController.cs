﻿using AutoMapper;
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
/// Controller to manage audio-related operations.
/// </summary>
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class AudioController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
    IWhisperService whisperService, IYouTubeService youTubeService, UserManager<User> userManager) : BaseController(unitOfWork, mapper)
{
    private readonly IFileService _fileService = fileService;
    private readonly IWhisperService _whisperService = whisperService;
    private readonly IYouTubeService _youTubeService = youTubeService;
    private readonly UserManager<User> _userManager = userManager;

    /// <summary>
    /// Uploads an audio file.
    /// </summary>
    /// <param name="file">The audio file to upload.</param>
    /// <returns>Action result of the upload operation.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Audio/upload
    ///     {
    ///        "file": "file content"
    ///     }
    ///
    /// </remarks>
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            return BadRequest("Could not get transcript, please try again");

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

        result.Audio = _mapper.Map<ResponseAudioDTO>(audio);

        return CreatedAtAction(nameof(GetAudioById), new { id = audio.Id }, _mapper.Map<ResponseAudioDTO>(audio));
    }

    /// <summary>
    /// Downloads audio from a YouTube link.
    /// </summary>
    /// <param name="link">The YouTube link to download audio from.</param>
    /// <returns>Action result of the download operation.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /Audio/youtubeLink?link=https://youtube.com/example
    ///
    /// </remarks>
    [HttpGet("youtubeLink")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadYoutubeAudio([FromQuery][Required] string link)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var filePath = await _fileService.DownloadFileAsync(FileSettings.AudiosPath, link, true);
        if (filePath is null)
            return BadRequest("Could not download audio file, please check the link or try again");

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
            new ResponseUploadFileDTO
            {
                Audio = _mapper.Map<ResponseAudioDTO>(audio),
                Link = audioLink,
                IsSuccess = true,
                Message = "Video Download Successfully",
            }
        );
    }

    /// <summary>
    /// Retrieves all audio files of the logged-in user.
    /// </summary>
    /// <returns>Action result containing the list of audio files.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /Audio/all
    ///
    /// </remarks>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAudios()
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audios = await _unitOfWork.Audios.GetAllByUserIdAsync(loggedinUser!.Id);

        var mappedAudios = audios.Select(_mapper.Map<ResponseAudioDTO>).ToList();

        return Ok(mappedAudios);
    }

    /// <summary>
    /// Retrieves an audio file by its ID.
    /// </summary>
    /// <param name="id">The ID of the audio file.</param>
    /// <returns>Action result containing the audio file.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /Audio/{id}
    ///
    /// </remarks>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAudioById([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audio = await _unitOfWork.Audios.GetByIdAsync(id);
        if (audio is null || audio.User.Id != loggedinUser!.Id)
            return BadRequest();

        var mappedAudio = _mapper.Map<ResponseAudioDTO>(audio);
        if (mappedAudio is null)
            return BadRequest();

        return Ok(mappedAudio);
    }

    /// <summary>
    /// Deletes an audio file by its ID.
    /// </summary>
    /// <param name="id">The ID of the audio file.</param>
    /// <returns>No content result.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /Audio/{id}
    ///
    /// </remarks>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAudio([FromRoute] int id)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var audio = await _unitOfWork.Audios.GetByIdToDeleteAsync(id);
        if (audio is null || audio.User.Id != loggedinUser!.Id)
            return BadRequest();

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
