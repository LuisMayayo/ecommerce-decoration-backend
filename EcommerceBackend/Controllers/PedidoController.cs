using Microsoft.AspNetCore.Mvc;

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

        // Obtener todos los pedidos de un usuario
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Pedido>>> GetByUserId(int userId)
        {
            var pedidos = await _pedidoService.GetByUserIdAsync(userId);
            if (pedidos == null || pedidos.Count == 0)
            {
                return NotFound("No se encontraron pedidos para este usuario.");
            }
            return Ok(pedidos);
        }

        // Obtener un pedido por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetById(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);
            if (pedido == null)
            {
                return NotFound("Pedido no encontrado.");
            }
            return Ok(pedido);
        }

        // Crear un nuevo pedido
        [HttpPost]
        public async Task<ActionResult<Pedido>> Create([FromBody] Pedido pedido)
        {
            pedido.FechaPedido = DateTime.UtcNow;  // Asignar la fecha del pedido automáticamente
            var newPedido = await _pedidoService.AddAsync(pedido);
            return CreatedAtAction(nameof(GetById), new { id = newPedido.Id }, newPedido);
        }
    }
}
