using Microsoft.AspNetCore.StaticFiles;
using NAudio.Wave;
using NReco.VideoConverter;
using NYoutubeDL;
using System.IO.Compression;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Settings;
using VideoToPostGenerationAPI.DTOs.Outgoing;
using VideoToPostGenerationAPI.Services.Helpers;

namespace VideoToPostGenerationAPI.Services;

public class FileService(IWebHostEnvironment env) : IFileService
{
    private readonly IWebHostEnvironment _env = env;

    public async Task<ResponseUploadFile> StoreAsync<TEnum>(IFormFile file, string filePath) where TEnum : struct, Enum
    {
        var extension = Path.GetExtension(file.FileName);

        if (!FileValidator.IsExtensionValid<TEnum>(extension))
            return new ResponseUploadFile
            {
                IsSuccess = false,
                Message = $"Extension is not Valid ({string.Join(',', FileValidator.GetValidExtensions<TEnum>())})",
            };

        long size = file.Length;
        if (!FileValidator.IsSizeValid(size))
            return new ResponseUploadFile
            {
                IsSuccess = false,
                Message = $"Maximum Size Can be {FileSettings.MaxFileSizeInGB}GB",
            };

        var fileName = Guid.NewGuid().ToString() + extension;
        var fullPath = Path.Combine(_env.WebRootPath, filePath);
        var handle = Path.Combine(fullPath, fileName);

        using FileStream stream = new(handle, FileMode.Create, FileAccess.ReadWrite);
        await file.CopyToAsync(stream);

        return new ResponseUploadFile
        {
            IsSuccess = true,
            Link = $"{filePath}{fileName}",
            Message = "File Upload Successfully",
        };
    }

    public async Task<byte[]> ReadAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        return await File.ReadAllBytesAsync(fullPath);
    }

    public void Delete(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return;

        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        File.Delete(fullPath);
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

    public async Task CompressAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        using var stream = File.Open(fullPath, FileMode.Open, FileAccess.ReadWrite);
        using var compressStream = new DeflateStream(stream, CompressionMode.Compress);

        await Task.CompletedTask;
    }

    public async Task DecompressAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        using var stream = File.Open(fullPath, FileMode.Open, FileAccess.ReadWrite);
        using var compressStream = new DeflateStream(stream, CompressionMode.Decompress);

        await Task.CompletedTask;
    }

    public async Task<string> ConvertVideoToAudioAsync(string filePath)
    {
        var inputFullPath = Path.Combine(_env.WebRootPath, filePath);

        var outputFileName = inputFullPath.Split('\\').Last().Split('.').First();

        var outputFullPath = Path.Combine(_env.WebRootPath, $"{FileSettings.AudiosPath}{outputFileName}") + ".wav";

        var ffMpeg = new FFMpegConverter();
        ffMpeg.ConvertMedia(inputFullPath, outputFullPath, "wav");

        return await Task.FromResult(outputFullPath);
    }

    public async Task<string?> DownloadFileAsync(string folderPath, string link, bool downloadAudio)
    {
        var filePath = Path.Combine(_env.WebRootPath, $"{folderPath}{Guid.NewGuid()}");

        var youtubeDl = new YoutubeDLP();

        youtubeDl.Options.FilesystemOptions.Output = filePath;
        youtubeDl.Options.PostProcessingOptions.ExtractAudio = downloadAudio;
        youtubeDl.VideoUrl = link;
        // Or update the binary
        //youtubeDl.Options.GeneralOptions.Update = true;

        // Optional, required if binary is not in $PATH
        //youtubeDl.YoutubeDlPath = filePath;

        //File.WriteAllText("options.config", youtubeDl.Options.Serialize());

        //youtubeDl.Options = Options.Deserialize(File.ReadAllText("options.config"));

        //List<string> s = [];

        //youtubeDl.StandardOutputEvent += (sender, output) =>
        //{
        //    s.Add(sender?.ToString() ?? "_");
        //    s.Add(output);
        //};
        //youtubeDl.StandardErrorEvent += (sender, errorOutput) =>
        //{
        //    s.Add(sender?.ToString() ?? "_");
        //    s.Add(errorOutput);
        //};

        //youtubeDl.Info.PropertyChanged += delegate { < your code here> };

        // Prepare the download (in case you need to validate the command before starting the download)
        //string commandToRun = await youtubeDl.PrepareDownloadAsync();
        //// Alternatively
        //string commandToRun = youtubeDl.PrepareDownload();

        // Just let it run
        await youtubeDl.DownloadAsync();

        // Wait for it
        //youtubeDl.Download();

        //// Or provide video url
        //youtubeDl.Download(link);

        return await GetFileByGuidIdAsync(filePath.Split('\\').Last(), folderPath);
    }

    public async Task<int> GetDurationAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        var ffProbe = new NReco.VideoInfo.FFProbe();
        var videoInfo = ffProbe.GetMediaInfo(fullPath);

        return await Task.FromResult((int)Math.Ceiling(videoInfo.Duration.TotalSeconds));
    }

    //public async Task<int> GetDurationAudioAsync(string filePath)
    //{
    //    var fullPath = Path.Combine(_env.WebRootPath, filePath);

    //    MediaFoundationReader mp3Stream = new(fullPath);
    //    TimeSpan timeSpan = mp3Stream.TotalTime;

    //    return await Task.FromResult((int)Math.Ceiling(timeSpan.TotalSeconds));
    //}
   
    public async Task<string?> GetContentTypeAsync(string filePath)
    {
        var fileProvider = new FileExtensionContentTypeProvider();
        if (!fileProvider.TryGetContentType(filePath, out string? contentType))
            return null;

        return await Task.FromResult(contentType);
    }

    public async Task<long> GetFileSizeAsync(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);

        FileInfo fileInfo = new(fullPath);

        return await Task.FromResult(fileInfo.Length);
    }

    public async Task<string?> GetFileByGuidIdAsync(string fileName, string FoldarPath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, FoldarPath);

        var files = Directory.GetFiles(fullPath).Select(file => file.Split('\\').Last()).ToList();

        var matchFile = files.SingleOrDefault(file => file.Contains(fileName));

        return await Task.FromResult(matchFile);
    }
}
