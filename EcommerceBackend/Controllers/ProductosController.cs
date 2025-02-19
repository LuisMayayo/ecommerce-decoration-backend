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
    /// Obtiene todos los productos
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ProductoDTO>>> GetAll()
    {
        var productos = await _productoService.GetAllAsync();
        return Ok(productos);
    }

    /// <summary>
    /// Obtiene un producto por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDTO>> GetById(int id)
    {
        var producto = await _productoService.GetByIdAsync(id);
        if (producto == null) return NotFound();
        return Ok(producto);
    }

    /// <summary>
    /// Obtiene productos filtrados por categoría.
    /// </summary>
    [HttpGet("categoria/{categoriaId}")]
    public async Task<ActionResult<List<ProductoDTO>>> GetByCategoria(int categoriaId)
    {
        var productos = await _productoService.GetByCategoriaIdAsync(categoriaId);
        return Ok(productos);
    }

    /// <summary>
    /// Crea un nuevo producto
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProductoDTO>> Create([FromBody] Producto producto)
    {
        producto.Validate();
        await _productoService.AddAsync(producto);
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto.ToDTO());
    }

    /// <summary>
    /// Actualiza un producto existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Producto producto)
    {
        var existingProducto = await _productoService.GetByIdAsync(id);
        if (existingProducto == null) return NotFound();

        producto.Id = id;
        await _productoService.UpdateAsync(producto);
        return NoContent();
    }

    /// <summary>
    /// Elimina un producto existente
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _productoService.GetByIdAsync(id);
        if (producto == null) return NotFound();

        await _productoService.DeleteAsync(id);
        return NoContent();
    }
    [HttpGet("dto")]
public async Task<ActionResult<List<ProductoDTO>>> GetAllDto()
{
    var productos = await _productoService.GetAllAsync();
    return Ok(productos);
}

}

}
