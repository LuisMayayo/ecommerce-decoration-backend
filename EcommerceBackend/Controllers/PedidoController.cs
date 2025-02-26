using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Data;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly EcommerceDbContext _context;

        public PedidoController(EcommerceDbContext context)
        {
            _context = context;
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
    }
}
