namespace VideoToPostGenerationAPI.Services.Helpers;

/// <summary>
/// Provides methods for converting Unix timestamps to DateTime objects.
/// </summary>
public static class UnixConverter
{
    /// <summary>
    /// Converts a Unix timestamp to a <see cref="DateTime"/> in UTC.
    /// </summary>
    /// <param name="unixDate">The Unix timestamp to convert. Represents the number of seconds elapsed since January 1, 1970 (the Unix epoch).</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the <see cref="DateTime"/> corresponding to the Unix timestamp.</returns>
    public static Task<DateTime> UnixTimeStampToDateTimeAsync(long unixDate)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        dateTime = dateTime.AddSeconds(unixDate).ToUniversalTime();

        return Task.FromResult(dateTime);
    }
}
