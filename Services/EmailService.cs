using System.Net;
using System.Net.Mail;
using System.Text;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;

namespace VideoToPostGenerationAPI.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var mail = new MailMessage
        {
            From = new MailAddress("eyaswannous@gmail.com", "Eyas Wannous", Encoding.UTF8),
            Subject = subject,
            SubjectEncoding = Encoding.UTF8,
            Body = body,
            BodyEncoding = Encoding.UTF8,
            IsBodyHtml = true,
            Priority = MailPriority.High
        };

        mail.To.Add(new MailAddress(toEmail));

        using var client = new SmtpClient
        {
            Credentials = new NetworkCredential("your email address", "your email password"),
            Port = 587,
            Host = "smtp.gmail.com",
            EnableSsl = true
        };

        try
        {
            await client.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending the email.");
        }
    }
}
