using System.Net.Mail;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using System.Text;
using System.Net;

namespace VideoToPostGenerationAPI.Services;

public class EmailService(ILogger<EmailService> logger) : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        MailMessage mail = new();
        mail.To.Add(new MailAddress(toEmail));
        mail.From = new MailAddress("eyaswannous@gmail.com", "Eyas Wannous", Encoding.UTF8);
        mail.Subject = subject;
        mail.SubjectEncoding = Encoding.UTF8;
        mail.Body = body;
        mail.BodyEncoding = Encoding.UTF8;
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;

        SmtpClient client = new()
        {
            Credentials = new NetworkCredential("from gmail address", "your gmail account password"),
            Port = 587,
            Host = "smtp.gmail.com",
            EnableSsl = true
        };

        try
        {
            client.Send(mail);
        }
        catch (Exception)
        {
            _logger.LogError("Something went wrong while sending the email");
        }

        await Task.FromResult(Task.CompletedTask);
    }
}