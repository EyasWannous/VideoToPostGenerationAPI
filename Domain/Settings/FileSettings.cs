namespace VideoToPostGenerationAPI.Domain.Settings;

public static class FileSettings
{
    public const string WebRootPath = "C:\\Users\\eyasw\\Desktop\\VideoToPostGenerationAPI\\wwwroot\\";
    public const string ImagesPath = "assets\\images\\"; // @"assets\images\"
    public const string AudiosPath = "assets\\audios\\"; // @"assets\audios\"
    public const string VideosPath = "assets\\videos\\"; // @"assets\videos\"
    //public const string AllowedExtensions = ".jpg,.png,.jpeg,.pdf";
    public const int MaxFileSizeInGB = 5;
    public const long MaxFileSizeInBytes = (long) MaxFileSizeInGB * 1024 * 1024 * 1024;
}

