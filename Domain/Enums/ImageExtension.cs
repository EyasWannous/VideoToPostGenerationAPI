namespace VideoToPostGenerationAPI.Domain.Enums;

/// <summary>
/// Represents different image file extensions.
/// </summary>
public enum ImageExtension
{
    None,
    /// <summary>
    /// Animated Portable Network Graphics format.
    /// </summary>
    apng,

    /// <summary>
    /// AV1 Image File Format, a new image format based on AV1 video codec.
    /// </summary>
    avif,

    /// <summary>
    /// Graphics Interchange Format, commonly used for animated images.
    /// </summary>
    gif,

    /// <summary>
    /// JPEG image format, a widely used method for lossy compression.
    /// </summary>
    jpg,

    /// <summary>
    /// JPEG image format, an alternative extension for JPEG images.
    /// </summary>
    jpeg,

    /// <summary>
    /// JPEG File Interchange Format, a variant of the JPEG format.
    /// </summary>
    jfif,

    /// <summary>
    /// Progressive JPEG format, used for higher quality images that load progressively.
    /// </summary>
    pjpeg,

    /// <summary>
    /// Progressive JPEG format, an alternative extension for progressive JPEG images.
    /// </summary>
    pjp,

    /// <summary>
    /// Portable Network Graphics format, known for lossless compression.
    /// </summary>
    png,

    /// <summary>
    /// Scalable Vector Graphics format, used for vector-based images.
    /// </summary>
    svg,

    /// <summary>
    /// WebP image format, known for providing good compression and quality.
    /// </summary>
    webp,

    /// <summary>
    /// Bitmap image format, used for storing pixel-based images.
    /// </summary>
    bmp,

    /// <summary>
    /// Icon format used for small icons and system icons.
    /// </summary>
    ico,

    /// <summary>
    /// Cursor format used for customizing the appearance of mouse pointers.
    /// </summary>
    cur,

    /// <summary>
    /// Tagged Image File Format, a flexible format for storing raster graphics.
    /// </summary>
    tif,

    /// <summary>
    /// Tagged Image File Format, an alternative extension for TIFF images.
    /// </summary>
    tiff
}
