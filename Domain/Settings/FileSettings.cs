namespace VideoToPostGenerationAPI.Domain.Settings;

public static class FileSettings
{
    public const string ImagesPath = "assets\\images\\";

    public const string AudiosPath = "assets\\audios\\";

    public const string VideosPath = "assets\\videos\\";

    public const int MaxFileSizeInGB = 5;

    public const long MaxFileSizeInBytes = MaxFileSizeInGB * 1024L * 1024L * 1024L;

    // public const string AllowedExtensions = ".jpg,.png,.jpeg,.pdf";
}
