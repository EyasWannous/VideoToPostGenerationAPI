﻿using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IFileService
{
    Task<ResponseUploadFileDTO> StoreAsync<TEnum>(IFormFile file, string filePath) where TEnum : struct, Enum;

    Task<byte[]> ReadAsync(string filePath);

    Task<IFormFile> MakeFileAsync(byte[] fileBytes, string fileName, string contentType);

    Task CompressAsync(string filePath);

    Task DecompressAsync(string filePath);

    Task<string> ConvertVideoToAudioAsync(string filePath);

    Task<string?> DownloadFileAsync(string folderPath, string link, bool downloadAudio);

    Task<int> GetDurationAsync(string filePath);

    Task<string?> GetContentTypeAsync(string link);

    Task<long> GetFileSizeAsync(string filePath);

    Task DeleteFileAsync(string filePath);

    Task<List<string>> GetAllFilesAsync(string folderPath);
    Task<string> DownloadImageAsync(string url);
}
