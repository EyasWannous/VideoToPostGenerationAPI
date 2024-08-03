namespace VideoToPostGenerationAPI.Domain.Enums;

/// <summary>
/// Represents different audio file extensions.
/// </summary>
public enum AudioExtension
{
    /// <summary>
    /// No specific audio extension.
    /// </summary>
    None,

    /// <summary>
    /// MPEG-1 Audio Layer 3 format.
    /// </summary>
    mp3,

    /// <summary>
    /// Advanced Audio Coding format.
    /// </summary>
    aac,

    /// <summary>
    /// Ogg Vorbis format.
    /// </summary>
    ogg,

    /// <summary>
    /// Free Lossless Audio Codec format.
    /// </summary>
    flac,

    /// <summary>
    /// Apple Lossless Audio Codec format.
    /// </summary>
    alac,

    /// <summary>
    /// Waveform Audio File format.
    /// </summary>
    wav,

    /// <summary>
    /// Audio Interchange File Format.
    /// </summary>
    aiff,

    /// <summary>
    /// Direct Stream Digital format.
    /// </summary>
    dsd,

    /// <summary>
    /// Pulse-Code Modulation format.
    /// </summary>
    pcm
}
