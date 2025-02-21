using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using EcommerceBackend.Models;

namespace EcommerceBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            // Remitente
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            // Destinatario
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            // Asunto
            message.Subject = subject;

            // Cuerpo del correo (texto plano; puedes usar HTML si quieres)
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            // Configurar el cliente SMTP
            using var client = new SmtpClient();
            try
            {
                // Conectarse al servidor SMTP
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                
                // Autenticarse
                await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);

                // Enviar el correo
                await client.SendAsync(message);

                // Desconectarse
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Manejar errores
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
                // Opcional: lanzar la excepción o registrarla
                throw;
            }
        }
    }
}
