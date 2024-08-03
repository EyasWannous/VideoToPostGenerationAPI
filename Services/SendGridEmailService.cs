using SendGrid.Helpers.Mail;
using SendGrid;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;

namespace VideoToPostGenerationAPI.Services;

/// <summary>
/// Service for sending emails using SendGrid.
/// </summary>
public class SendGridEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendGridEmailService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration used to retrieve the SendGrid API key.</param>
    public SendGridEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Sends an email asynchronously using SendGrid.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="content">The content of the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method uses the SendGrid API to send the email. Ensure that the SendGrid API key is 
    /// correctly configured in the application settings.
    /// </remarks>
    public async Task SendEmailAsync(string toEmail, string subject, string content)
    {
        var apiKey = _configuration["SendGridAPIKey"];
        var client = new SendGridClient(apiKey);

        var from = new EmailAddress("test@example.com", "BudgetBlitz");
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

        var response = await client.SendEmailAsync(msg);

        // You can add additional handling here if necessary, such as checking the response status.
    }
}
