using VideoToPostGenerationAPI.Domain.Enums;

namespace VideoToPostGenerationAPI.Services.Helpers;

public static class FileExtensionsHelper
{
    public static TEnum? GetExtension<TEnum>(string extension) where TEnum : struct, Enum
    {
        if (!Enum.TryParse(extension, out TEnum result))
            return null;

        return result;
    }

    public static string? IsExtension<TEnum>(string extension) where TEnum : struct, Enum
    {
        if (!Enum.TryParse(extension, out TEnum result))
            return null;

        return result.ToString();
    }
}
