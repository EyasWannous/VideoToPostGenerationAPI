using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Controllers;

/// <summary>
/// Controller for handling operations related to audio and video files, including uploading, downloading, and retrieving details.
/// </summary>
public class FakeController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeController"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance for data access.</param>
    /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>
    public FakeController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    /// <summary>
    /// Uploads an audio file.
    /// </summary>
    /// <param name="file">The audio file to upload.</param>
    /// <returns>Returns a response with the created audio's DTO.</returns>
    [HttpPost("audio/upload")]
    public IActionResult UploadAudio([Required] IFormFile file)
    {
        var audio = new Audio
        {
            SizeBytes = file.Length,
            AudioExtension = ".mp3",
            Transcript = "transcript",
            Link = "assets/audios/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp3",
            Duration = 2132,
            UserId = 0,
        };

        return CreatedAtAction(nameof(GetAudioById), new { id = audio.Id }, _mapper.Map<ResponseAudioDTO>(audio));
    }

    /// <summary>
    /// Downloads audio from a YouTube link.
    /// </summary>
    /// <param name="link">The YouTube link from which to download audio.</param>
    /// <returns>Returns a response with the uploaded file's DTO and link.</returns>
    [HttpGet("audio/youtubeLink")]
    public IActionResult DownloadYoutubeAudio([FromQuery][Required] string link = "https://www.youtube.com/watch?v=MYdKBuA3XF8")
    {
        var audio = new Audio
        {
            SizeBytes = 123,
            AudioExtension = ".mp3",
            Transcript = "transcript",
            Link = "assets/audios/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp3",
            Duration = 2132,
            UserId = 0,
        };

        return Ok(new ResponseUploadFileDTO
        {
            Audio = _mapper.Map<ResponseAudioDTO>(audio),
            Link = "assets/videos/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp3",
            IsSuccess = true,
            Message = "Video Download Successfully",
        });
    }

    /// <summary>
    /// Retrieves all audio files.
    /// </summary>
    /// <returns>Returns a list of all audio DTOs.</returns>
    [HttpGet("audio/all")]
    public IActionResult GetAllAudios()
    {
        var audio = new Audio
        {
            SizeBytes = 123123,
            AudioExtension = ".mp3",
            Transcript = "transcript",
            Link = "assets/audios/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp3",
            Duration = 2132,
            UserId = 0,
        };

        var audios = new List<Audio> { audio, audio };
        var mappedAudios = audios.Select(_mapper.Map<ResponseAudioDTO>).ToList();

        return Ok(mappedAudios);
    }

    /// <summary>
    /// Retrieves an audio file by its ID.
    /// </summary>
    /// <param name="id">The ID of the audio file to retrieve.</param>
    /// <returns>Returns the audio DTO with the specified ID.</returns>
    [HttpGet("audio/{id:int}")]
    public IActionResult GetAudioById([FromRoute] int id)
    {
        var audio = new Audio
        {
            SizeBytes = 123123,
            AudioExtension = ".mp3",
            Transcript = "transcript",
            Link = "assets/audios/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp3",
            Duration = 2132,
            UserId = 0,
        };

        var mappedAudio = _mapper.Map<ResponseAudioDTO>(audio);

        return Ok(mappedAudio);
    }

    /// <summary>
    /// Deletes an audio file by its ID.
    /// </summary>
    /// <param name="id">The ID of the audio file to delete.</param>
    /// <returns>Returns a NoContent result if successful.</returns>
    [HttpDelete("audio/{id:int}")]
    public IActionResult DeleteAudio([FromRoute] int id)
    {
        return NoContent();
    }

    // Video

    /// <summary>
    /// Uploads a video file.
    /// </summary>
    /// <param name="file">The video file to upload.</param>
    /// <returns>Returns a response with the created video's DTO.</returns>
    [HttpPost("video/upload")]
    public IActionResult UploadVideo([Required] IFormFile file)
    {
        var audio = new Audio
        {
            SizeBytes = 123123,
            AudioExtension = ".mp3",
            Transcript = "transcript",
            Link = "assets/audios/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp3",
            Duration = 2132,
            UserId = 0,
        };

        var video = new Video
        {
            SizeBytes = file.Length,
            Link = "assets/videos/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp4",
            VideoExtension = ".mp4",
            Audio = audio,
            AudioId = audio.Id,
        };

        audio.Video = video;

        return CreatedAtAction(nameof(GetVideoById), new { id = video.Id }, _mapper.Map<ResponseVideoDTO>(video));
    }

    /// <summary>
    /// Downloads a video from a YouTube link.
    /// </summary>
    /// <param name="link">The YouTube link from which to download the video.</param>
    /// <returns>Returns a response with the uploaded file's DTO and link.</returns>
    [HttpGet("video/youtubeLink")]
    public IActionResult DownloadYoutubeVideo([FromQuery][Required] string link = "https://www.youtube.com/watch?v=MYdKBuA3XF8")
    {
        var audio = new Audio
        {
            SizeBytes = 123123,
            AudioExtension = ".mp3",
            Transcript = "transcript",
            Link = "assets/audios/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp3",
            Duration = 2132,
            UserId = 0,
        };

        var video = new Video
        {
            SizeBytes = 231231,
            Link = "assets/videos/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp4",
            VideoExtension = ".mp4",
            Audio = audio,
            AudioId = audio.Id,
        };

        audio.Video = video;

        return Ok(new ResponseUploadFileDTO
        {
            Audio = _mapper.Map<ResponseAudioDTO>(audio),
            Video = _mapper.Map<ResponseVideoDTO>(video),
            Link = "assets/videos/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp4",
            IsSuccess = true,
            Message = "Video Download Successfully",
        });
    }

    /// <summary>
    /// Retrieves all video files.
    /// </summary>
    /// <returns>Returns a list of all video DTOs.</returns>
    [HttpGet("video/all")]
    public IActionResult GetAllVideos()
    {
        var audio = new Audio
        {
            SizeBytes = 123123,
            AudioExtension = ".mp3",
            Transcript = "transcript",
            Link = "assets/audios/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp3",
            Duration = 2132,
            UserId = 0,
        };

        var video = new Video
        {
            SizeBytes = 231231,
            Link = "assets/videos/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp4",
            VideoExtension = ".mp4",
            Audio = audio,
            AudioId = audio.Id,
        };

        var videos = new List<Video> { video, video };
        var mappedVideos = videos.Select(_mapper.Map<ResponseVideoDTO>).ToList();

        return Ok(mappedVideos);
    }

    /// <summary>
    /// Retrieves a video file by its ID.
    /// </summary>
    /// <param name="id">The ID of the video file to retrieve.</param>
    /// <returns>Returns the video DTO with the specified ID.</returns>
    [HttpGet("video/{id:int}")]
    public IActionResult GetVideoById([FromRoute] int id)
    {
        var audio = new Audio
        {
            SizeBytes = 123123,
            AudioExtension = ".mp3",
            Transcript = "transcript",
            Link = "assets/audios/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp3",
            Duration = 2132,
            UserId = 0,
        };

        var video = new Video
        {
            SizeBytes = 231231,
            Link = "assets/videos/1064335a-e58f-45d8-bc25-edd90ddb5b17.mp4",
            VideoExtension = ".mp4",
            Audio = audio,
            AudioId = audio.Id,
        };

        var mappedVideo = _mapper.Map<ResponseVideoDTO>(video);

        return Ok(mappedVideo);
    }

    /// <summary>
    /// Deletes a video file by its ID.
    /// </summary>
    /// <param name="id">The ID of the video file to delete.</param>
    /// <returns>Returns a NoContent result if successful.</returns>
    [HttpDelete("video/{id:int}")]
    public IActionResult DeleteVideo([FromRoute] int id)
    {
        return NoContent();
    }
}
