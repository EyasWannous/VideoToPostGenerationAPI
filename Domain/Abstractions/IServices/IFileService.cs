using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IFileService
{
    public Task<ResponseUploadFileDTO> StoreAsync<TEnum>(IFormFile file, string filePath) where TEnum : struct, Enum;
    public Task<byte[]> ReadAsync(string filePath);
    public Task<IFormFile> MakeFileAsync(byte[] fileBytes, string fileName, string contentType);
    public Task CompressAsync(string filePath);
    public Task DecompressAsync(string filePath);
    public Task<string> ConvertVideoToAudioAsync(string filePath);
    public Task<string?> DownloadFileAsync(string folderPath, string link, bool downloadAudio);
    public Task<int> GetDurationAsync(string filePath);
    public Task<string?> GetContentTypeAsync(string link);
    public Task<long> GetFileSizeAsync(string filePath);
    public Task DeleteFileAsync(string filePath);
}
