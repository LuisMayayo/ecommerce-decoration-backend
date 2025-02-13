using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReseñaController : ControllerBase
    {
        private readonly IReseñaService _reseñaService;

        public ReseñaController(IReseñaService reseñaService)
        {
            _reseñaService = reseñaService;
        }

        // Obtener todas las reseñas de un producto
        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<List<Reseña>>> GetByProductoId(int productoId)
        {
            var reseñas = await _reseñaService.GetByProductoIdAsync(productoId);
            if (reseñas == null || reseñas.Count == 0)
            {
                return NotFound("No se encontraron reseñas para este producto.");
            }
            return Ok(reseñas);
        }

        // Agregar una nueva reseña
        [HttpPost]
        public async Task<ActionResult<Reseña>> Create([FromBody] Reseña reseña)
        {
            var newReseña = await _reseñaService.AddAsync(reseña);
            return CreatedAtAction(nameof(GetByProductoId), new { productoId = newReseña.ProductoId }, newReseña);
        }
    }
}
