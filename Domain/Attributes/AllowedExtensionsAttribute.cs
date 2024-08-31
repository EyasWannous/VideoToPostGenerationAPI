using System.ComponentModel.DataAnnotations;

namespace VideoToPostGenerationAPI.Domain.Attributes;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _allowedExtensions;

    public AllowedExtensionsAttribute(params string[] allowedExtensions)
    {
        _allowedExtensions = allowedExtensions;
    }

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
