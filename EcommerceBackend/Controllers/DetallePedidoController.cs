using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Data;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidoController : ControllerBase
    {
        private readonly EcommerceDbContext _context;

        public DetallePedidoController(EcommerceDbContext context)
        {
            _context = context;
        }

        [HttpGet("pedido/{pedidoId}")]
        public async Task<ActionResult<List<DetallePedido>>> GetByPedidoId(int pedidoId)
        {
            var detalles = await _context.DetallesPedido
                .Include(d => d.Pedido)
                    .ThenInclude(p => p.Usuario)  
                .Include(d => d.Producto) 
                    .ThenInclude(p => p.Categoria) 
                .Where(d => d.PedidoId == pedidoId)
                .ToListAsync();

            if (detalles == null || detalles.Count == 0)
                return NotFound("No se encontraron detalles para este pedido.");

            return Ok(detalles);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] List<DetallePedido> detallesPedido)
        {
            if (detallesPedido == null || detallesPedido.Count == 0)
                return BadRequest("No se proporcionaron detalles de pedido.");

            foreach (var detalle in detallesPedido)
            {
                var pedidoExiste = await _context.Pedidos.AnyAsync(p => p.Id == detalle.PedidoId);
                var productoExiste = await _context.Productos.AnyAsync(p => p.Id == detalle.ProductoId);

                if (!pedidoExiste)
                    return BadRequest($"Pedido con ID {detalle.PedidoId} no encontrado.");
                if (!productoExiste)
                    return BadRequest($"Producto con ID {detalle.ProductoId} no encontrado.");

                await _context.DetallesPedido.AddAsync(detalle);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Detalles de pedido agregados correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var detalle = await _context.DetallesPedido.FindAsync(id);
            if (detalle == null)
                return NotFound("Detalle de pedido no encontrado.");

            _context.DetallesPedido.Remove(detalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
