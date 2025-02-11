using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase
{
    private static List<Producto> _productos = new List<Producto>
    {
        new Producto { Id = 1, Nombre = "Lámpara Moderna", Descripcion = "Lámpara de diseño", Precio = 45.99m, Stock = 10 },
        new Producto { Id = 2, Nombre = "Cojín Decorativo", Descripcion = "Cojín de tela suave", Precio = 19.99m, Stock = 20 }
    };

    [HttpGet]
    public IActionResult GetProductos()
    {
        return Ok(_productos);
    }

    [HttpGet("{id}")]
    public IActionResult GetProducto(int id)
    {
        var producto = _productos.Find(p => p.Id == id);
        if (producto == null) return NotFound();
        return Ok(producto);
    }

    [HttpPost]
    public IActionResult CreateProducto([FromBody] Producto producto)
    {
        if (producto == null) return BadRequest();
        producto.Id = _productos.Count + 1;
        _productos.Add(producto);
        return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProducto(int id, [FromBody] Producto productoActualizado)
    {
        var producto = _productos.Find(p => p.Id == id);
        if (producto == null) return NotFound();
        
        producto.Nombre = productoActualizado.Nombre;
        producto.Descripcion = productoActualizado.Descripcion;
        producto.Precio = productoActualizado.Precio;
        producto.Stock = productoActualizado.Stock;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProducto(int id)
    {
        var producto = _productos.Find(p => p.Id == id);
        if (producto == null) return NotFound();
        
        _productos.Remove(producto);
        return NoContent();
    }
}
