using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private static List<Pedido> _pedidos = new List<Pedido>
    {
        new Pedido { Id = 1, UsuarioId = 1, Total = 99.99m, FechaPedido = DateTime.Now },
        new Pedido { Id = 2, UsuarioId = 2, Total = 59.50m, FechaPedido = DateTime.Now }
    };

    // Obtener todos los pedidos
    [HttpGet]
    public IActionResult GetPedidos()
    {
        return Ok(_pedidos);
    }

    // Obtener un pedido por ID
    [HttpGet("{id}")]
    public IActionResult GetPedido(int id)
    {
        var pedido = _pedidos.Find(p => p.Id == id);
        if (pedido == null) return NotFound();
        return Ok(pedido);
    }

    // Crear un nuevo pedido
    [HttpPost]
    public IActionResult CreatePedido([FromBody] Pedido pedido)
    {
        if (pedido == null) return BadRequest();
        pedido.Id = _pedidos.Count + 1;
        pedido.FechaPedido = DateTime.Now;
        _pedidos.Add(pedido);
        return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
    }

    // Actualizar un pedido existente
    [HttpPut("{id}")]
    public IActionResult UpdatePedido(int id, [FromBody] Pedido pedidoActualizado)
    {
        var pedido = _pedidos.Find(p => p.Id == id);
        if (pedido == null) return NotFound();
        
        pedido.Total = pedidoActualizado.Total;
        return NoContent();
    }

    // Eliminar un pedido
    [HttpDelete("{id}")]
    public IActionResult DeletePedido(int id)
    {
        var pedido = _pedidos.Find(p => p.Id == id);
        if (pedido == null) return NotFound();
        
        _pedidos.Remove(pedido);
        return NoContent();
    }
}
