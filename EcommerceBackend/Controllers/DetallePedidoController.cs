using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Services;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidoController : ControllerBase
    {
        private readonly IDetallePedidoService _detallePedidoService;

        public DetallePedidoController(IDetallePedidoService detallePedidoService)
        {
            _detallePedidoService = detallePedidoService;
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
        public async Task<ActionResult<DetallePedido>> Create([FromBody] DetallePedido detallePedido)
        {
            await _detallePedidoService.AddAsync(detallePedido);
            return CreatedAtAction(nameof(GetByPedidoId), new { pedidoId = detallePedido.PedidoId }, detallePedido);
        }
    }
}
