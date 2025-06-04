using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using EcommerceBackend.Models;
using EcommerceBackend.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EcommerceBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly EcommerceDbContext _context;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, EcommerceDbContext context, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _context = context;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                _logger.LogInformation($"Intentando enviar email a: {toEmail}");
                
                // Validar configuración
                if (string.IsNullOrEmpty(_emailSettings.Host) || 
                    string.IsNullOrEmpty(_emailSettings.UserName) || 
                    string.IsNullOrEmpty(_emailSettings.Password))
                {
                    throw new InvalidOperationException("Configuración de email incompleta");
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress(toEmail, toEmail));
                message.Subject = subject;

                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using var client = new SmtpClient();
                
                // Configurar timeouts
                client.Timeout = 30000; // 30 segundos
                
                try
                {
                    _logger.LogInformation($"Conectando a SMTP: {_emailSettings.Host}:{_emailSettings.Port}");
                    
                    await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                    
                    _logger.LogInformation("Conexión SMTP establecida, autenticando...");
                    await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                    
                    _logger.LogInformation("Autenticación exitosa, enviando mensaje...");
                    await client.SendAsync(message);
                    
                    _logger.LogInformation($"Email enviado exitosamente a: {toEmail}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error específico de SMTP: {ex.Message}");
                    throw new InvalidOperationException($"Error de conectividad SMTP: {ex.Message}", ex);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        await client.DisconnectAsync(true);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando email a {toEmail}: {ex.Message}");
                throw new InvalidOperationException($"No se pudo enviar el email: {ex.Message}", ex);
            }
        }

        public async Task SendOrderConfirmationAsync(int pedidoId, string userEmail, string userName)
        {
            try
            {
                _logger.LogInformation($"Enviando confirmación de pedido {pedidoId} a {userEmail}");
                
                var pedido = await _context.Pedidos
                    .Include(p => p.Usuario)
                    .FirstOrDefaultAsync(p => p.Id == pedidoId);

                if (pedido == null)
                    throw new Exception($"Pedido con ID {pedidoId} no encontrado.");

                var detalles = await _context.DetallesPedido
                    .Include(d => d.Producto)
                    .Where(d => d.PedidoId == pedidoId)
                    .ToListAsync();

                if (detalles == null || !detalles.Any())
                    throw new Exception($"No se encontraron detalles para el pedido {pedidoId}.");

                var bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine($"<h2>¡Hola {userName}!</h2>");
                bodyBuilder.AppendLine($"<p>Gracias por tu compra. Tu pedido <strong>#{pedidoId}</strong> ha sido recibido y está siendo procesado.</p>");
                bodyBuilder.AppendLine("<h3>Detalles del pedido:</h3>");
                bodyBuilder.AppendLine("<table border='1' style='border-collapse: collapse; width: 100%;'>");
                bodyBuilder.AppendLine("<tr><th>Producto</th><th>Cantidad</th><th>Precio Unitario</th><th>Subtotal</th></tr>");
                
                decimal total = 0;
                foreach (var detalle in detalles)
                {
                    var subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    total += subtotal;
                    bodyBuilder.AppendLine($"<tr><td>{detalle.Producto.Nombre}</td><td>{detalle.Cantidad}</td><td>{detalle.PrecioUnitario:C}</td><td>{subtotal:C}</td></tr>");
                }
                
                bodyBuilder.AppendLine("</table>");
                bodyBuilder.AppendLine($"<h3>Total del pedido: {total:C}</h3>");
                bodyBuilder.AppendLine($"<p><strong>Fecha del pedido:</strong> {pedido.FechaPedido.ToString("dd/MM/yyyy HH:mm")}</p>");
                bodyBuilder.AppendLine("<p>Si tienes alguna pregunta sobre tu pedido, no dudes en contactarnos.</p>");
                bodyBuilder.AppendLine("<p>Saludos,<br/>El equipo de LM Decoraciones</p>");

                await SendEmailAsync(
                    userEmail,
                    $"Confirmación de Pedido #{pedidoId} - LM DECORACIONES",
                    bodyBuilder.ToString()
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando confirmación de pedido {pedidoId}: {ex.Message}");
                throw new InvalidOperationException($"Error al enviar correo de confirmación: {ex.Message}", ex);
            }
        }
    }
}