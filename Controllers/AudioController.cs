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
using VideoToPostGenerationAPI.Services.Helpers;

namespace VideoToPostGenerationAPI.Controllers;

[Authorize]
public class AudioController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
    UserManager<User> userManager) : BaseController(unitOfWork, mapper)
{
    private readonly IFileService _fileService = fileService;
    private readonly UserManager<User> _userManager = userManager;

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var result = await _fileService.StoreAsync<AudioExtension>(file, $"{FileSettings.AudiosPath}"); 
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        var extension = Path.GetExtension(result.Link);
        var duration = await _fileService.GetDurationAsync(result.Link);

        var audio = new Audio
        {
            SizeBytes = file.Length,
            AudioExtension = FileExtensionsHelper.GetExtension<AudioExtension>(extension.Split('.').Last()) ?? AudioExtension.None,
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
    public async Task<IActionResult> DownloadYoutubeAudio(string link)
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);

        var filePath = await _fileService.DownloadFileAsync(FileSettings.AudiosPath, link, true);
        if (filePath is null)
            return BadRequest();

        var audioLink = $"{FileSettings.AudiosPath}{filePath}";

        var audioExtension = Path.GetExtension(audioLink);
        var duration = await _fileService.GetDurationAsync(audioLink);

        var audio = new Audio
        {
            SizeBytes = await _fileService.GetFileSizeAsync(audioLink),
            AudioExtension = FileExtensionsHelper.GetExtension<AudioExtension>(audioExtension.Split('.').Last()) ?? AudioExtension.None,
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

}
