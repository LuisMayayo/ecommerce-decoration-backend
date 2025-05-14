using Microsoft.AspNetCore.Mvc;
using System;
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

        // GET api/Producto
        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> GetAll()
        {
            try
            {
                var productos = await _productoService.GetAllAsync();
                var dtos = productos.ConvertAll(p => p.ToDto());
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener productos: {ex.Message}" });
            }
        }

        // GET api/Producto/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            try
            {
                var producto = await _productoService.GetByIdAsync(id);
                return Ok(producto.ToDto());
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener el producto: {ex.Message}" });
            }
        }

        // GET api/Producto/categoria/{categoriaId}
        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<List<ProductoDto>>> GetByCategoria(int categoriaId)
        {
            try
            {
                var productos = await _productoService.GetByCategoriaIdAsync(categoriaId);
                var dtos = productos.ConvertAll(p => p.ToDto());
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener productos por categoría: {ex.Message}" });
            }
        }

        // POST api/Producto
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create([FromBody] Producto producto)
        {
            try
            {
                await _productoService.AddAsync(producto);
                return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al crear el producto: {ex.Message}" });
            }
        }

        // PUT api/Producto/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Producto producto)
        {
            try
            {
                producto.Id = id;
                await _productoService.UpdateAsync(producto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al actualizar el producto: {ex.Message}" });
            }
        }

        // DELETE api/Producto/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al eliminar el producto: {ex.Message}" });
            }
        }
    }
}