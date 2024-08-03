namespace VideoToPostGenerationAPI.DTOs.Outgoing;

/// <summary>
/// Represents the data transfer object for file upload responses.
/// </summary>
public record ResponseUploadFileDTO
{
    /// <summary>
    /// Gets or sets the message associated with the file upload response.
    /// This can provide additional information about the upload result.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the link or URL to the uploaded file.
    /// </summary>
    public string Link { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the file upload was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the audio response data associated with the file upload, if applicable.
    /// This property may be null if the uploaded file is not an audio file.
    /// </summary>
    public ResponseAudioDTO? Audio { get; set; }

    /// <summary>
    /// Gets or sets the video response data associated with the file upload, if applicable.
    /// This property may be null if the uploaded file is not a video file.
    /// </summary>
    public ResponseVideoDTO? Video { get; set; }
}
