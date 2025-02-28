// Services/PedidoService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Repositories;

namespace EcommerceBackend.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IEmailService _emailService;

        public PedidoService(IPedidoRepository pedidoRepository, IEmailService emailService)
        {
            _pedidoRepository = pedidoRepository;
            _emailService = emailService;
        }

        public async Task<List<Pedido>> GetByUserIdAsync(int userId)
        {
            return await _pedidoRepository.GetByUserIdAsync(userId);
        }

        public async Task<Pedido> GetByIdAsync(int id)
        {
            return await _pedidoRepository.GetByIdAsync(id);
        }

        public async Task<Pedido> CreateAsync(Pedido pedido)
        {
            pedido.FechaPedido = DateTime.UtcNow;
            var nuevoPedido = await _pedidoRepository.CreateAsync(pedido);
            
            try
            {
                var pedidoConUsuario = await _pedidoRepository.GetByIdWithUserAsync(nuevoPedido.Id);
                if (pedidoConUsuario?.Usuario != null)
                {
                    await _emailService.SendOrderConfirmationAsync(
                        pedidoConUsuario.Id,
                        pedidoConUsuario.Usuario.Email,
                        pedidoConUsuario.Usuario.Nombre
                    );
                }
            }
            catch (Exception ex)
            {
                // Loguear el error, pero no afectar la creación del pedido
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
            }
            
            return nuevoPedido;
        }

        public async Task DeleteAsync(int id)
        {
            await _pedidoRepository.DeleteAsync(id);
        }

        public async Task EnviarConfirmacionPedidoAsync(int pedidoId)
        {
            var pedido = await _pedidoRepository.GetByIdWithUserAsync(pedidoId);
            
            if (pedido == null)
                throw new KeyNotFoundException($"Pedido con ID {pedidoId} no encontrado.");

            if (pedido.Usuario == null)
                throw new InvalidOperationException("El pedido no tiene un usuario asociado.");

            await _emailService.SendOrderConfirmationAsync(
                pedidoId, 
                pedido.Usuario.Email, 
                pedido.Usuario.Nombre
            );
        }
    }
}
