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

        public EmailService(IOptions<EmailSettings> emailSettings, EcommerceDbContext context)
        {
            _emailSettings = emailSettings.Value;
            _context = context;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                
                await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);

                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
                throw;
            }
        }

        public async Task SendOrderConfirmationAsync(int pedidoId, string userEmail, string userName)
        {
            try
            {
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
                bodyBuilder.AppendLine($"¡Hola {userName}!");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine($"Gracias por tu compra. Tu pedido #{pedidoId} ha sido recibido y está siendo procesado.");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine("Detalles del pedido:");
                bodyBuilder.AppendLine("-------------------");
                
                decimal total = 0;
                foreach (var detalle in detalles)
                {
                    var subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    total += subtotal;
                    bodyBuilder.AppendLine($"- {detalle.Producto.Nombre}: {detalle.Cantidad} x {detalle.PrecioUnitario:C} = {subtotal:C}");
                }
                
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine($"Total del pedido: {total:C}");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine($"Fecha del pedido: {pedido.FechaPedido.ToString("dd/MM/yyyy HH:mm")}");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine("Si tienes alguna pregunta sobre tu pedido, no dudes en contactarnos.");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine("Saludos,");
                bodyBuilder.AppendLine("El equipo de Mi Tienda");

                await SendEmailAsync(
                    userEmail,
                    $"Confirmación de Pedido #{pedidoId} - LM DECORACIONES",
                    bodyBuilder.ToString()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo de confirmación: {ex.Message}");
                throw;
            }
        }
    }
}
