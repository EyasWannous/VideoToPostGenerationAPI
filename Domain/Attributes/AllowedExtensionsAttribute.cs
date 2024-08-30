using System.ComponentModel.DataAnnotations;

namespace VideoToPostGenerationAPI.Domain.Attributes;

/// <summary>
/// Custom validation attribute to enforce allowed file extensions for file uploads.
/// </summary>
public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _allowedExtensions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllowedExtensionsAttribute"/> class.
    /// </summary>
    /// <param name="allowedExtensions">The allowed file extensions, including the leading dot (e.g., ".jpg", ".png").</param>
    public AllowedExtensionsAttribute(params string[] allowedExtensions)
    {
        _allowedExtensions = allowedExtensions;
    }

    /// <summary>
    /// Validates the file extension.
    /// </summary>
    /// <param name="value">The object to validate, typically an <see cref="IFormFile"/>.</param>
    /// <param name="validationContext">The context of the validation.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating whether the validation succeeded or failed.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
            return ValidationResult.Success; // No file to validate; validation is considered successful.

        var extension = Path.GetExtension(file.FileName);

        var isAllowed = _allowedExtensions
            .Contains(extension, StringComparer.OrdinalIgnoreCase);

        if (!isAllowed)
        {
            var allowedExtensionsString = string.Join(", ", _allowedExtensions);
            return new ValidationResult($"Only the following file extensions are allowed: {allowedExtensionsString}.");
        }

        return ValidationResult.Success; // File extension is allowed.
    }
}
