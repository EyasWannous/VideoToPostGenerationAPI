namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

/// <summary>
/// Defines methods for sending emails.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="toEmail">The email address of the recipient.</param>
    /// <param name="subject">The subject line of the email.</param>
    /// <param name="body">The body content of the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailAsync(string toEmail, string subject, string body);
}
