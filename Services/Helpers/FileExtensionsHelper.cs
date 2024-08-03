namespace VideoToPostGenerationAPI.Services.Helpers;

/// <summary>
/// Helper class for file extension operations.
/// </summary>
public static class FileExtensionsHelper
{
    /// <summary>
    /// Attempts to get the enum value corresponding to the provided extension string.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum to parse.</typeparam>
    /// <param name="extension">The extension string to parse.</param>
    /// <returns>The corresponding enum value if parsing is successful; otherwise, null.</returns>
    public static TEnum? GetExtension<TEnum>(string extension) where TEnum : struct, Enum
    {
        if (Enum.TryParse(extension, ignoreCase: true, out TEnum result))
            return result;

        return null;
    }

    /// <summary>
    /// Returns the string representation of the enum value corresponding to the provided extension string.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum to parse.</typeparam>
    /// <param name="extension">The extension string to parse.</param>
    /// <returns>The string representation of the enum value if parsing is successful; otherwise, null.</returns>
    public static string? IsExtension<TEnum>(string extension) where TEnum : struct, Enum
    {
        var enumValue = GetExtension<TEnum>(extension);
        return enumValue?.ToString();
    }
}
