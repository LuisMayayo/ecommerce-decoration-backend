using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.DTOs;
using EcommerceBackend.Extensions;
using EcommerceBackend.Services;

namespace EcommerceBackend.Controllers
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

        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> GetAll()
        {
            var productos = await _productoService.GetAllAsync();
            var dtos = productos.ConvertAll(p => p.ToDto());
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null) return NotFound();
            return Ok(producto.ToDto());
        }

        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<List<ProductoDto>>> GetByCategoria(int categoriaId)
        {
            var productos = await _productoService.GetByCategoriaIdAsync(categoriaId);
            var dtos = productos.ConvertAll(p => p.ToDto());
            return Ok(dtos);
        }

        // Nuevo endpoint para filtrar por nombre de producto
        [HttpGet("search")]
        public async Task<ActionResult<List<ProductoDto>>> SearchByName([FromQuery] string query)
        {
            var productos = await _productoService.SearchByNameAsync(query);
            var dtos = productos.ConvertAll(p => p.ToDto());
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create([FromBody] Producto producto)
        {
            producto.Validate();
            await _productoService.AddAsync(producto);
            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Producto producto)
        {
            var existing = await _productoService.GetByIdAsync(id);
            if (existing == null) return NotFound();
            producto.Id = id;
            await _productoService.UpdateAsync(producto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null) return NotFound();
            await _productoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
