namespace VideoToPostGenerationAPI.Services.Helpers;

public static class FileExtensionsHelper
{
    public static TEnum? GetExtension<TEnum>(string extension) where TEnum : struct, Enum
    {
        if (Enum.TryParse(extension, ignoreCase: true, out TEnum result))
            return result;

        return null;
    }

    public static string? IsExtension<TEnum>(string extension) where TEnum : struct, Enum
    {
        var enumValue = GetExtension<TEnum>(extension);
        return enumValue?.ToString();
    }
}
