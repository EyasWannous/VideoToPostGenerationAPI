namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}
