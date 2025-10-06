using BLL.Model.SendModels;
using BLL.Service.Interface;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace BLL.Service.ServiceHelpers;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(
            _config["EmailSettings:SenderName"],
            _config["EmailSettings:SenderEmail"]));

        foreach (var emailRecipient in message.To)
            email.To.Add(MailboxAddress.Parse(emailRecipient));

        email.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.IsHtml ? message.Content : null,
            TextBody = !message.IsHtml ? message.Content : null
        };

        email.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"],
            int.Parse(_config["EmailSettings:Port"]),
            MailKit.Security.SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(_config["EmailSettings:Username"],
            _config["EmailSettings:Password"]);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}