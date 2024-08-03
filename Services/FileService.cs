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

/// <summary>
/// Service for handling file operations including upload, read, delete, compress, decompress, and video-to-audio conversion.
/// </summary>
public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileService"/> class.
    /// </summary>
    /// <param name="env">The web hosting environment.</param>
    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    /// <summary>
    /// Stores a file asynchronously in the specified path.
    /// </summary>
    /// <typeparam name="TEnum">The type of enum used to validate file extensions.</typeparam>
    /// <param name="file">The file to store.</param>
    /// <param name="filePath">The path where the file should be stored.</param>
    /// <returns>A <see cref="ResponseUploadFileDTO"/> indicating the result of the operation.</returns>
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

    /// <summary>
    /// Reads a file's contents asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file to read.</param>
    /// <returns>A byte array containing the file's contents.</returns>
    public async Task<byte[]> ReadAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);
        return await File.ReadAllBytesAsync(fullPath);
    }

    /// <summary>
    /// Deletes a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file to delete.</param>
    public async Task DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return;

        var fullPath = Path.Combine(_env.WebRootPath, filePath);
        File.Delete(fullPath);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Creates a <see cref="IFormFile"/> from a byte array asynchronously.
    /// </summary>
    /// <param name="fileBytes">The byte array representing the file's contents.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <returns>A task representing the asynchronous operation, with an <see cref="IFormFile"/> as the result.</returns>
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

    /// <summary>
    /// Compresses a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file to compress.</param>
    public async Task CompressAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        using var stream = File.Open(fullPath, FileMode.Open, FileAccess.ReadWrite);
        using var compressStream = new DeflateStream(stream, CompressionMode.Compress);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Decompresses a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file to decompress.</param>
    public async Task DecompressAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        using var stream = File.Open(fullPath, FileMode.Open, FileAccess.ReadWrite);
        using var compressStream = new DeflateStream(stream, CompressionMode.Decompress);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Converts a video file to an audio file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the video file to convert.</param>
    /// <returns>A task representing the asynchronous operation, with the path to the converted audio file as the result.</returns>
    public async Task<string> ConvertVideoToAudioAsync(string filePath)
    {
        var inputFullPath = Path.Combine(_env.WebRootPath, filePath);
        var outputFileName = inputFullPath.Split('\\').Last().Split('.').First();
        var outputFullPath = Path.Combine(_env.WebRootPath, FileSettings.AudiosPath, outputFileName) + ".wav";

        var ffMpeg = new FFMpegConverter();
        ffMpeg.ConvertMedia(inputFullPath, outputFullPath, "wav");

        return await Task.FromResult(outputFullPath);
    }

    /// <summary>
    /// Downloads a file from a URL asynchronously.
    /// </summary>
    /// <param name="folderPath">The folder path where the file should be stored.</param>
    /// <param name="link">The URL of the file to download.</param>
    /// <param name="downloadAudio">Indicates whether to download audio instead of video.</param>
    /// <returns>A task representing the asynchronous operation, with the path to the downloaded file as the result.</returns>
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
            VideoUrl = link
        };

        var videoInfo = await youtubeDl.GetDownloadInfoAsync();
        if (videoInfo is not VideoDownloadInfo castVideoInfo)
            return null;

        long? fileSize = castVideoInfo.RequestedFormats.Max(format => format.Filesize);

        if (fileSize is null || fileSize > FileSettings.MaxFileSizeInBytes)
            return null;

        await youtubeDl.DownloadAsync();

        return await GetFileByGuidIdAsync(fileGuid, folderPath);
    }

    /// <summary>
    /// Gets the duration of a video file in seconds asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the video file.</param>
    /// <returns>A task representing the asynchronous operation, with the duration in seconds as the result.</returns>
    public async Task<int> GetDurationAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        var ffProbe = new NReco.VideoInfo.FFProbe();
        var videoInfo = ffProbe.GetMediaInfo(fullPath);

        return await Task.FromResult((int)Math.Ceiling(videoInfo.Duration.TotalSeconds));
    }

    /// <summary>
    /// Gets the content type of a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>A task representing the asynchronous operation, with the content type of the file as the result.</returns>
    public async Task<string?> GetContentTypeAsync(string filePath)
    {
        var fileProvider = new FileExtensionContentTypeProvider();
        if (!fileProvider.TryGetContentType(filePath, out string? contentType))
            return null;

        return await Task.FromResult(contentType);
    }

    /// <summary>
    /// Gets the size of a file in bytes asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>A task representing the asynchronous operation, with the file size in bytes as the result.</returns>
    public async Task<long> GetFileSizeAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);
        var fileInfo = new FileInfo(fullPath);

        return await Task.FromResult(fileInfo.Length);
    }

    /// <summary>
    /// Gets a file name by its GUID from the specified folder path asynchronously.
    /// </summary>
    /// <param name="fileName">The GUID part of the file name to search for.</param>
    /// <param name="folderPath">The folder path to search in.</param>
    /// <returns>A task representing the asynchronous operation, with the matching file name as the result.</returns>
    public async Task<string?> GetFileByGuidIdAsync(string fileName, string folderPath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, folderPath);
        var files = Directory.GetFiles(fullPath).Select(file => file.Split('\\').Last()).ToList();
        var matchFile = files.SingleOrDefault(file => file.Contains(fileName));

        return await Task.FromResult(matchFile);
    }
}
