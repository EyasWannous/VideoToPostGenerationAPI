using System.ComponentModel.DataAnnotations;

namespace VideoToPostGenerationAPI.Domain.Attributes;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;

    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
            return ValidationResult.Success; // No file to validate; validation is considered successful.

        if (file.Length > _maxFileSize)
            return new ValidationResult($"File size can't exceed {_maxFileSize} bytes.");

        return ValidationResult.Success; // File size is within the acceptable range.
    }
}
