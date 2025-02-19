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
        /// <param name="isDto">Determina si se retorna como DTO o como modelo completo.</param>
        /// <returns>Lista de productos.</returns>
        [HttpGet]
        public async Task<ActionResult<List<ProductoDTO>>> GetAll([FromQuery] int? categoriaId, [FromQuery] bool isDto = false)
        {
            // Si isDto es verdadero, retorna productos como ProductoDTO
            if (isDto)
            {
                var productos = await _productoService.GetAllAsync(); // Retorna ProductoDTO
                return Ok(productos);
            }

            // Si no hay filtro de categoría, devuelve todos los productos como Producto
            if (!categoriaId.HasValue)
            {
                var productosAll = await _productoService.GetAllAsync(); // Obtener productos normales
                return Ok(productosAll);
            }

            // Si hay filtro de categoría, devuelve solo los productos de esa categoría
            var productosPorCategoria = await _productoService.GetByCategoriaIdAsync(categoriaId.Value);
            return Ok(productosPorCategoria);
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

        // Este método ahora devuelve una lista de ProductoDTO en lugar de Producto
        [HttpGet("dto")]
        public async Task<ActionResult<List<ProductoDTO>>> GetAllDto()
        {
            var productos = await _productoService.GetAllAsync();
            return Ok(productos);
        }

        // Método para obtener productos por categoria
        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<List<ProductoDTO>>> GetByCategoria(int categoriaId)
        {
            var productos = await _productoService.GetByCategoriaIdAsync(categoriaId);
            return Ok(productos);
        }
    }
}
