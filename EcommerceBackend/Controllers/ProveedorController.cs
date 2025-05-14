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
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedorService _proveedorService;
        
        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }
        
        // GET api/Proveedor
        [HttpGet]
        public async Task<ActionResult<List<ProveedorDto>>> GetAll()
        {
            try
            {
                var proveedores = await _proveedorService.GetAllAsync();
                var dtos = proveedores.ConvertAll(p => p.ToDto());
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener proveedores: {ex.Message}" });
            }
        }
        
        // GET api/Proveedor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDto>> GetById(int id)
        {
            try
            {
                var proveedor = await _proveedorService.GetByIdAsync(id);
                return Ok(proveedor.ToDto());
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener el proveedor: {ex.Message}" });
            }
        }
        
        // GET api/Proveedor/{id}/productos
        [HttpGet("{id}/productos")]
        public async Task<ActionResult<List<ProductoDto>>> GetProductosByProveedor(int id)
        {
            try
            {
                var productos = await _proveedorService.GetProductosByProveedorIdAsync(id);
                var dtos = productos.ConvertAll(p => p.ToDto());
                return Ok(dtos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener productos del proveedor: {ex.Message}" });
            }
        }
        
        // POST api/Proveedor
        [HttpPost]
        public async Task<ActionResult<ProveedorDto>> Create([FromBody] Proveedor proveedor)
        {
            try
            {
                int id = await _proveedorService.AddAsync(proveedor);
                // Recargar para obtener el objeto completo
                var createdProveedor = await _proveedorService.GetByIdAsync(id);
                return CreatedAtAction(nameof(GetById), new { id = createdProveedor.Id }, createdProveedor.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al crear el proveedor: {ex.Message}" });
            }
        }
        
        // PUT api/Proveedor/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Proveedor proveedor)
        {
            try
            {
                proveedor.Id = id;
                await _proveedorService.UpdateAsync(proveedor);
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
                return StatusCode(500, new { error = $"Error al actualizar el proveedor: {ex.Message}" });
            }
        }
        
        // DELETE api/Proveedor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _proveedorService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al eliminar el proveedor: {ex.Message}" });
            }
        }
    }
}