﻿using SendGrid.Helpers.Mail;
using SendGrid;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;

namespace VideoToPostGenerationAPI.Services;

public class SendGridEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public SendGridEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

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
