// Controllers/PedidoController.cs
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Services;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Pedido>>> GetByUserId(int userId)
        {
            var pedidos = await _pedidoService.GetByUserIdAsync(userId);

            if (pedidos == null || pedidos.Count == 0)
                return NotFound("No se encontraron pedidos para este usuario.");

            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetById(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);

            if (pedido == null) return NotFound("Pedido no encontrado.");
            return Ok(pedido);
        }
        [HttpGet]
        public async Task<ActionResult<List<Pedido>>> GetAll()
        {
            var pedidos = await _pedidoService.GetAllAsync();
            return Ok(pedidos);
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> Create([FromBody] Pedido pedido)
        {
            var nuevoPedido = await _pedidoService.CreateAsync(pedido);
            return CreatedAtAction(nameof(GetById), new { id = nuevoPedido.Id }, nuevoPedido);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _pedidoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Pedido no encontrado.");
            }
        }

        [HttpPost("enviar-confirmacion/{pedidoId}")]
        public async Task<IActionResult> EnviarConfirmacionPedido(int pedidoId)
        {
            try
            {
                await _pedidoService.EnviarConfirmacionPedidoAsync(pedidoId);
                return Ok(new { mensaje = "Te hemos enviado al correo toda la información de tu pedido." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al enviar correo: {ex.Message}" });
            }
        }
    }
}
