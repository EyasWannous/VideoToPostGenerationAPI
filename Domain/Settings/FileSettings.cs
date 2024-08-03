namespace VideoToPostGenerationAPI.Domain.Settings;

/// <summary>
/// Contains settings related to file paths and size limits used in the application.
/// </summary>
public static class FileSettings
{
    /// <summary>
    /// The relative path to the directory where image files are stored.
    /// </summary>
    /// <remarks>
    /// This path is relative to the root of the web application.
    /// Uses forward slashes for compatibility across different operating systems.
    /// </remarks>
    public const string ImagesPath = "assets\\images\\";

    /// <summary>
    /// The relative path to the directory where audio files are stored.
    /// </summary>
    /// <remarks>
    /// This path is relative to the root of the web application.
    /// Uses forward slashes for compatibility across different operating systems.
    /// </remarks>
    public const string AudiosPath = "assets\\audios\\";

    /// <summary>
    /// The relative path to the directory where video files are stored.
    /// </summary>
    /// <remarks>
    /// This path is relative to the root of the web application.
    /// Uses forward slashes for compatibility across different operating systems.
    /// </remarks>
    public const string VideosPath = "assets\\videos\\";

    /// <summary>
    /// The maximum allowable file size in gigabytes.
    /// </summary>
    /// <remarks>
    /// This value is used to calculate the maximum file size allowed in bytes.
    /// </remarks>
    public const int MaxFileSizeInGB = 5;

    /// <summary>
    /// The maximum allowable file size in bytes.
    /// </summary>
    /// <remarks>
    /// This value is calculated from <see cref="MaxFileSizeInGB"/> by converting gigabytes to bytes.
    /// The calculation uses a long literal to ensure the value fits within a long data type.
    /// </remarks>
    public const long MaxFileSizeInBytes = MaxFileSizeInGB * 1024L * 1024 * 1024;

    ///// <summary>
    ///// The allowed file extensions for uploads.
    ///// </summary>
    ///// <remarks>
    ///// This field is currently commented out and serves as a placeholder for allowed file extensions.
    ///// The extensions can be defined as a comma-separated list of file types.
    ///// </remarks>
    // public const string AllowedExtensions = ".jpg,.png,.jpeg,.pdf";
}
