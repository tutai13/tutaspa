using API.DTOs.Auth;
using API.IService;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using MimeKit;

namespace API.Services
{
    public class GmailService : IGmailService
    {
        private readonly GmailSettings gmailSettings;

        public GmailService(IOptions<GmailSettings> option)
        {
            gmailSettings = option.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("No Reply", gmailSettings.GmailAddress));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(gmailSettings.GmailAddress, gmailSettings.AppPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
