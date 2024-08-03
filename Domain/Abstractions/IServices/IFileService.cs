using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

/// <summary>
/// Defines methods for managing files including storage, retrieval, and manipulation.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Stores a file asynchronously.
    /// </summary>
    /// <typeparam name="TEnum">An enumeration type used for additional file processing, such as file type or category.</typeparam>
    /// <param name="file">The file to be stored.</param>
    /// <param name="filePath">The path where the file should be stored.</param>
    /// <returns>A task that represents the asynchronous operation, with a <see cref="ResponseUploadFileDTO"/> result containing information about the uploaded file.</returns>
    Task<ResponseUploadFileDTO> StoreAsync<TEnum>(IFormFile file, string filePath) where TEnum : struct, Enum;

    /// <summary>
    /// Reads a file asynchronously and returns its content as a byte array.
    /// </summary>
    /// <param name="filePath">The path to the file to be read.</param>
    /// <returns>A task that represents the asynchronous operation, with a <see cref="byte"/> result containing the file's content.</returns>
    Task<byte[]> ReadAsync(string filePath);

    /// <summary>
    /// Creates an <see cref="IFormFile"/> instance from byte array data.
    /// </summary>
    /// <param name="fileBytes">The byte array containing file data.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <returns>A task that represents the asynchronous operation, with an <see cref="IFormFile"/> result.</returns>
    Task<IFormFile> MakeFileAsync(byte[] fileBytes, string fileName, string contentType);

    /// <summary>
    /// Compresses a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file to be compressed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CompressAsync(string filePath);

    /// <summary>
    /// Decompresses a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file to be decompressed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DecompressAsync(string filePath);

    /// <summary>
    /// Converts a video file to an audio file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the video file to be converted.</param>
    /// <returns>A task that represents the asynchronous operation, with a <see cref="string"/> result containing the path to the converted audio file.</returns>
    Task<string> ConvertVideoToAudioAsync(string filePath);

    /// <summary>
    /// Downloads a file asynchronously from a specified link and folder path.
    /// </summary>
    /// <param name="folderPath">The folder path where the file will be saved.</param>
    /// <param name="link">The link from which to download the file.</param>
    /// <param name="downloadAudio">A flag indicating whether the file to be downloaded is an audio file.</param>
    /// <returns>A task that represents the asynchronous operation, with a <see cref="string"/> result containing the path to the downloaded file.</returns>
    Task<string?> DownloadFileAsync(string folderPath, string link, bool downloadAudio);

    /// <summary>
    /// Gets the duration of a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file whose duration is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation, with an <see cref="int"/> result representing the duration in seconds.</returns>
    Task<int> GetDurationAsync(string filePath);

    /// <summary>
    /// Gets the content type of a file asynchronously from a specified link.
    /// </summary>
    /// <param name="link">The link to the file.</param>
    /// <returns>A task that represents the asynchronous operation, with a <see cref="string"/> result containing the MIME type of the file.</returns>
    Task<string?> GetContentTypeAsync(string link);

    /// <summary>
    /// Gets the size of a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file whose size is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation, with a <see cref="long"/> result representing the file size in bytes.</returns>
    Task<long> GetFileSizeAsync(string filePath);

    /// <summary>
    /// Deletes a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteFileAsync(string filePath);
}
