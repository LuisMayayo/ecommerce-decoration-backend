using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        /// <summary>
        /// Obtiene todos los productos o los productos filtrados por categoría.
        /// </summary>
        /// <param name="categoriaId">ID de la categoría para filtrar los productos (opcional).</param>
        /// <returns>Lista de productos.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Producto>>> GetAll([FromQuery] int? categoriaId)
        {
            if (categoriaId.HasValue)
            {
                // Filtrar los productos por categoriaId
                var productos = await _productoService.GetByCategoriaIdAsync(categoriaId.Value);
                return Ok(productos);
            }

            // Si no hay filtro, retorna todos los productos
            var productosAll = await _productoService.GetAllAsync();
            return Ok(productosAll);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetById(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="producto">Objeto Producto a crear.</param>
        /// <returns>Producto creado.</returns>
        [HttpPost]
        public async Task<ActionResult<Producto>> Create([FromBody] Producto producto)
        {
            await _productoService.AddAsync(producto);
            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="id">ID del producto a actualizar.</param>
        /// <param name="producto">Objeto Producto con los datos actualizados.</param>
        /// <returns>Estado de la operación.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Producto producto)
        {
            var existingProducto = await _productoService.GetByIdAsync(id);
            if (existingProducto == null)
            {
                return NotFound();
            }
            await _productoService.UpdateAsync(producto);
            return NoContent();
        }

        /// <summary>
        /// Elimina un producto existente.
        /// </summary>
        /// <param name="id">ID del producto a eliminar.</param>
        /// <returns>Estado de la operación.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            await _productoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
