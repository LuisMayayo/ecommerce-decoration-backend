using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Services;
using EcommerceBackend.Data; // Agregar la referencia al contexto de la base de datos

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidoController : ControllerBase
    {
        private readonly IDetallePedidoService _detallePedidoService;
        private readonly EcommerceDbContext _context; // Inyectamos el contexto de la base de datos

        public DetallePedidoController(IDetallePedidoService detallePedidoService, EcommerceDbContext context)
        {
            _detallePedidoService = detallePedidoService;
            _context = context; // Asignamos el contexto
        }

        [HttpGet("pedido/{pedidoId}")]
        public async Task<ActionResult<List<DetallePedido>>> GetByPedidoId(int pedidoId)
        {
            var detalles = await _detallePedidoService.GetByPedidoIdAsync(pedidoId);
            if (detalles == null || detalles.Count == 0)
                return NotFound("No se encontraron detalles para este pedido.");
            return Ok(detalles);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] List<DetallePedido> detallesPedido)
        {
            try
            {
                if (detallesPedido == null || detallesPedido.Count == 0)
                    return BadRequest("No se proporcionaron detalles de pedido.");

                foreach (var detalle in detallesPedido)
                {
                    // Verificar si el PedidoId y ProductoId existen en la base de datos
                    var pedidoExiste = await _context.Pedidos.AnyAsync(p => p.Id == detalle.PedidoId);
                    var productoExiste = await _context.Productos.AnyAsync(p => p.Id == detalle.ProductoId);

                    if (!pedidoExiste)
                        return BadRequest($"Pedido con ID {detalle.PedidoId} no encontrado.");

                    if (!productoExiste)
                        return BadRequest($"Producto con ID {detalle.ProductoId} no encontrado.");

                    await _detallePedidoService.AddAsync(detalle);
                }

                return Ok(new { message = "Detalles de pedido agregados correctamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error en la API:", ex);
                return StatusCode(500, "Error interno en el servidor: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _detallePedidoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
