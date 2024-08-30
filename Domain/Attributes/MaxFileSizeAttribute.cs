using System.ComponentModel.DataAnnotations;

namespace VideoToPostGenerationAPI.Domain.Attributes;

/// <summary>
/// Custom validation attribute to enforce a maximum file size limit.
/// </summary>
public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaxFileSizeAttribute"/> class.
    /// </summary>
    /// <param name="maxFileSize">The maximum allowable file size in bytes.</param>
    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    /// <summary>
    /// Validates the file size.
    /// </summary>
    /// <param name="value">The object to validate, typically an <see cref="IFormFile"/>.</param>
    /// <param name="validationContext">The context of the validation.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating whether the validation succeeded or failed.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
            return ValidationResult.Success; // No file to validate; validation is considered successful.

        if (file.Length > _maxFileSize)
            return new ValidationResult($"File size can't exceed {_maxFileSize} bytes.");

        return ValidationResult.Success; // File size is within the acceptable range.
    }
}
