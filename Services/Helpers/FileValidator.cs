using VideoToPostGenerationAPI.Domain.Settings;

namespace VideoToPostGenerationAPI.Services.Helpers;

/// <summary>
/// Helper class for validating file extensions and sizes.
/// </summary>
public static class FileValidator
{
    /// <summary>
    /// Gets a list of valid file extensions based on the provided enum type.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum representing valid file extensions.</typeparam>
    /// <returns>A list of valid file extensions with a leading dot.</returns>
    public static List<string> GetValidExtensions<TEnum>() where TEnum : struct, Enum
    {
        var validExtensions = new List<string>();
        foreach (var item in Enum.GetValues<TEnum>())
        {
            validExtensions.Add('.' + item.ToString());
        }

        return validExtensions;
    }

    /// <summary>
    /// Checks if the provided file extension is valid based on the provided enum type.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum representing valid file extensions.</typeparam>
    /// <param name="extension">The file extension to check, including the leading dot.</param>
    /// <returns>True if the extension is valid; otherwise, false.</returns>
    public static bool IsExtensionValid<TEnum>(string? extension) where TEnum : struct, Enum
    {
        if (extension == null)
            return false;

        var validExtensions = new HashSet<string>(GetValidExtensions<TEnum>(), StringComparer.OrdinalIgnoreCase);
        return validExtensions.Contains(extension);
    }

    /// <summary>
    /// Checks if the provided file size is valid based on the maximum allowed size.
    /// </summary>
    /// <param name="size">The file size in bytes.</param>
    /// <returns>True if the file size is valid; otherwise, false.</returns>
    public static bool IsSizeValid(long size)
        => size < FileSettings.MaxFileSizeInBytes;
}
