namespace VideoToPostGenerationAPI.Domain.Entities;

/// <summary>
/// Represents an audio file entity.
/// </summary>
public class Audio : BaseFile
{
    /// <summary>
    /// Gets or sets the title of the audio file.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file extension of the audio.
    /// </summary>
    public string AudioExtension { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the duration of the audio in seconds.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the transcript of the audio.
    /// </summary>
    public string Transcript { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the YouTube link associated with the audio.
    /// </summary>
    public string YoutubeLink { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the associated video entity.
    /// </summary>
    public Video? Video { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user who owns the audio file.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the user entity associated with the audio.
    /// </summary>
    public User User { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of posts associated with the audio.
    /// </summary>
    public List<Post> Posts { get; set; } = [];
    
    public VideoThumbnail VideoThumbnail { get; set; }

    public bool HasVideo { get; set; }
}
