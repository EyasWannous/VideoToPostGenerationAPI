using Microsoft.AspNetCore.StaticFiles;
using NReco.VideoConverter;
using NYoutubeDL;
using NYoutubeDL.Models;
using System.IO.Compression;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Settings;
using VideoToPostGenerationAPI.DTOs.Outgoing;
using VideoToPostGenerationAPI.Services.Helpers;

namespace VideoToPostGenerationAPI.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private readonly HttpClient _client;

    public FileService(IWebHostEnvironment env, HttpClient client)
    {
        _env = env;
        _client = client;
    }

    public async Task<ResponseUploadFileDTO> StoreAsync<TEnum>(IFormFile file, string filePath) where TEnum : struct, Enum
    {
        var extension = Path.GetExtension(file.FileName);

        if (!FileValidator.IsExtensionValid<TEnum>(extension))
            return new ResponseUploadFileDTO
            {
                IsSuccess = false,
                Message = $"Extension is not valid ({string.Join(',', FileValidator.GetValidExtensions<TEnum>())})",
            };

        long size = file.Length;
        if (!FileValidator.IsSizeValid(size))
            return new ResponseUploadFileDTO
            {
                IsSuccess = false,
                Message = $"Maximum size can be {FileSettings.MaxFileSizeInGB} GB",
            };

        var fileName = Guid.NewGuid().ToString() + extension;
        var fullPath = Path.Combine(_env.WebRootPath, filePath);
        var handle = Path.Combine(fullPath, fileName);

        using (FileStream stream = new(handle, FileMode.Create, FileAccess.ReadWrite))
        {
            await file.CopyToAsync(stream);
        }

        return new ResponseUploadFileDTO
        {
            IsSuccess = true,
            Link = $"{filePath}{fileName}",
            Message = "File uploaded successfully",
        };
    }

    public async Task<byte[]> ReadAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);
        return await File.ReadAllBytesAsync(fullPath);
    }

    public Task DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return Task.CompletedTask;

        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }

    public async Task<IFormFile> MakeFileAsync(byte[] fileBytes, string fileName, string contentType)
    {
        var memoryStream = new MemoryStream(fileBytes);
        var file = new FormFile(memoryStream, 0, fileBytes.Length, "File", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };

        return await Task.FromResult(file);
    }

    public Task CompressAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        using var stream = File.Open(fullPath, FileMode.Open, FileAccess.ReadWrite);
        using var compressStream = new DeflateStream(stream, CompressionMode.Compress);

        return Task.CompletedTask;
    }

    public Task DecompressAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        using var stream = File.Open(fullPath, FileMode.Open, FileAccess.ReadWrite);
        using var compressStream = new DeflateStream(stream, CompressionMode.Decompress);

        return Task.CompletedTask;
    }

    public Task<string> ConvertVideoToAudioAsync(string filePath)
    {
        var inputFullPath = Path.Combine(_env.WebRootPath, filePath);
        var outputFileName = inputFullPath.Split('\\').Last().Split('.').First();
        var outputFullPath = Path.Combine(_env.WebRootPath, FileSettings.AudiosPath, outputFileName) + ".wav";

        var ffMpeg = new FFMpegConverter();
        ffMpeg.ConvertMedia(inputFullPath, outputFullPath, "wav");

        return Task.FromResult(outputFullPath);
    }

    public async Task<string?> DownloadFileAsync(string folderPath, string link, bool downloadAudio)
    {
        var fileGuid = Guid.NewGuid().ToString();
        var filePath = Path.Combine(_env.WebRootPath, folderPath, fileGuid);
        var filePathTemplate = Path.Combine(_env.WebRootPath, folderPath, $"{fileGuid}.%(ext)s");

        var youtubeDl = new YoutubeDLP
        {
            Options = {
                FilesystemOptions = { Output = filePathTemplate },
                PostProcessingOptions = { ExtractAudio = downloadAudio }
            },
            VideoUrl = link,
            YoutubeDlPath = Path.Combine(_env.WebRootPath, "yt-dlp.exe")
        };

        var videoInfo = await youtubeDl.GetDownloadInfoAsync();
        if (videoInfo is not VideoDownloadInfo castVideoInfo)
            return null;
        //castVideoInfo.Tags;
        //castVideoInfo.Thumbnail;
        //castVideoInfo.Duration;
        //castVideoInfo.Categories;
        //castVideoInfo.Description;
        //castVideoInfo.Title;

        long? fileSize = castVideoInfo.RequestedFormats.Max(format => format.Filesize);

        if (fileSize is null || fileSize > FileSettings.MaxFileSizeInBytes)
            return null;

        await youtubeDl.DownloadAsync();

        return await GetFileByGuidIdAsync(fileGuid, folderPath);
    }

    public Task<int> GetDurationAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        var ffProbe = new NReco.VideoInfo.FFProbe();
        var videoInfo = ffProbe.GetMediaInfo(fullPath);

        return Task.FromResult((int)Math.Ceiling(videoInfo.Duration.TotalSeconds));
    }

    public Task<string?> GetContentTypeAsync(string filePath)
    {
        var fileProvider = new FileExtensionContentTypeProvider();
        if (!fileProvider.TryGetContentType(filePath, out string? contentType))
            return Task.FromResult<string?>(null);

        return Task.FromResult<string?>(contentType);
    }

    public Task<long> GetFileSizeAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);
        var fileInfo = new FileInfo(fullPath);

        return Task.FromResult(fileInfo.Length);
    }

    public Task<string?> GetFileByGuidIdAsync(string fileName, string folderPath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, folderPath);
        var files = Directory.GetFiles(fullPath).Select(file => file.Split('\\').Last()).ToList();
        var matchFile = files.SingleOrDefault(file => file.Contains(fileName));

        return Task.FromResult(matchFile);
    }

    public Task<List<string>> GetAllFilesAsync(string folderPath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, folderPath);
        var files = Directory.GetFiles(fullPath);

        return Task.FromResult(files.ToList());
    }

    public async Task<string> DownloadImageAsync(string url)
    {
        var filePath = Path.Combine(_env.WebRootPath, FileSettings.ImagesPath);

        byte[] imageBytes = await _client.GetByteArrayAsync(url);

        var imageName = Guid.NewGuid().ToString() + ".jpg";

        var fullPath = Path.Combine(filePath, imageName);

        await File.WriteAllBytesAsync(fullPath, imageBytes);

        return imageName;
    }
}
