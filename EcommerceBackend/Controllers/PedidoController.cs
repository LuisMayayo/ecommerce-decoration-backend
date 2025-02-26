using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Services;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Pedido>>> GetByUserId(int userId)
        {
            var pedidos = await _pedidoService.GetByUserIdAsync(userId);
            if (pedidos == null || pedidos.Count == 0)
                return NotFound("No se encontraron pedidos para este usuario.");
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetById(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);
            if (pedido == null) return NotFound("Pedido no encontrado.");
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> Create([FromBody] Pedido pedido)
        {
            pedido.FechaPedido = DateTime.UtcNow;
            var newPedido = await _pedidoService.AddAsync(pedido);
            return CreatedAtAction(nameof(GetById), new { id = newPedido.Id }, newPedido);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pedidoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
