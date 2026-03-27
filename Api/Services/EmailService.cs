using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace Api.Services
{
    public interface IEmailService
    {
        Task EnviarCorreoAsync(string destinatario, string asunto, string mensajeHtml);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string mensajeHtml)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_config["EmailConfig:SenderName"] ?? "Mi Tienda", _config["EmailConfig:SenderEmail"] ?? "no-reply@mitienda.com"));
            email.To.Add(MailboxAddress.Parse(destinatario));
            email.Subject = asunto;

            var builder = new BodyBuilder { HtmlBody = mensajeHtml };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["EmailConfig:SmtpServer"], _config.GetValue<int>("EmailConfig:Port"), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["EmailConfig:SenderEmail"], _config["EmailConfig:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
