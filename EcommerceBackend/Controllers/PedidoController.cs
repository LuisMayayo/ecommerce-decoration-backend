using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Data;
using EcommerceBackend.Services;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly EcommerceDbContext _context;
        private readonly IEmailService _emailService;

        public PedidoController(EcommerceDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        /// <summary>
        /// Obtener pedidos por usuario ID
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Pedido>>> GetByUserId(int userId)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario) // Incluye la información del usuario
                .Where(p => p.UsuarioId == userId)
                .ToListAsync();

            if (pedidos == null || pedidos.Count == 0)
                return NotFound("No se encontraron pedidos para este usuario.");

            return Ok(pedidos);
        }

        /// <summary>
        /// Obtener un pedido por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetById(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario) // Incluye la información del usuario
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null) return NotFound("Pedido no encontrado.");
            return Ok(pedido);
        }

        /// <summary>
        /// Crear un nuevo pedido
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Pedido>> Create([FromBody] Pedido pedido)
        {
            pedido.FechaPedido = DateTime.UtcNow;
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            
            // Intentar enviar correo de confirmación automáticamente
            try
            {
                var usuario = await _context.Usuarios.FindAsync(pedido.UsuarioId);
                if (usuario != null)
                {
                    await _emailService.SendOrderConfirmationAsync(
                        pedido.Id,
                        usuario.Email,
                        usuario.Nombre
                    );
                }
            }
            catch (Exception ex)
            {
                // Solo loguear el error, no afectar la creación del pedido
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
            }
            
            return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
        }

        /// <summary>
        /// Eliminar un pedido por ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound("Pedido no encontrado.");

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        /// <summary>
        /// Enviar correo de confirmación de pedido
        /// </summary>
        [HttpPost("enviar-confirmacion/{pedidoId}")]
        public async Task<IActionResult> EnviarConfirmacionPedido(int pedidoId)
        {
            try
            {
                var pedido = await _context.Pedidos
                    .Include(p => p.Usuario)
                    .FirstOrDefaultAsync(p => p.Id == pedidoId);

                if (pedido == null)
                    return NotFound($"Pedido con ID {pedidoId} no encontrado.");

                await _emailService.SendOrderConfirmationAsync(
                    pedidoId, 
                    pedido.Usuario.Email, 
                    pedido.Usuario.Nombre
                );

                return Ok(new { mensaje = "Te hemos enviado al correo toda la información de tu pedido." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al enviar correo: {ex.Message}" });
            }
        }
    }
}
