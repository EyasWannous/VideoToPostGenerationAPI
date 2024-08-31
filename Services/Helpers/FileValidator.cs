using VideoToPostGenerationAPI.Domain.Settings;

namespace VideoToPostGenerationAPI.Services.Helpers;

public static class FileValidator
{
    public static List<string> GetValidExtensions<TEnum>() where TEnum : struct, Enum
    {
        var validExtensions = new List<string>();
        foreach (var item in Enum.GetValues<TEnum>())
        {
            validExtensions.Add('.' + item.ToString());
        }

        return validExtensions;
    }

    public static bool IsExtensionValid<TEnum>(string? extension) where TEnum : struct, Enum
    {
        if (extension == null)
            return false;

        var validExtensions = new HashSet<string>(GetValidExtensions<TEnum>(), StringComparer.OrdinalIgnoreCase);
        return validExtensions.Contains(extension);
    }

    public static bool IsSizeValid(long size)
        => size < FileSettings.MaxFileSizeInBytes;
}
