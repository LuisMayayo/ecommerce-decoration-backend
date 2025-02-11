using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductos()
    {
        var productos = await _productoService.GetAll();
        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProducto(int id)
    {
        var producto = await _productoService.GetById(id);
        if (producto == null) return NotFound();
        return Ok(producto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProducto([FromBody] Producto producto)
    {
        await _productoService.Add(producto);
        return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProducto(int id, [FromBody] Producto producto)
    {
        var existingProducto = await _productoService.GetById(id);
        if (existingProducto == null) return NotFound();

        producto.Id = id;
        await _productoService.Update(producto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProducto(int id)
    {
        var existingProducto = await _productoService.GetById(id);
        if (existingProducto == null) return NotFound();

        await _productoService.Delete(id);
        return NoContent();
    }
}
