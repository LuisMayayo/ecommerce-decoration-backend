using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using EcommerceBackend.Models;
using EcommerceBackend.DTOs;
using EcommerceBackend.Services;
using EcommerceBackend.Extensions;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");
            return Ok(usuario.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            var userClaimId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "0");
            if (userClaimId != id && !User.IsInRole("Admin"))
                return Forbid("No tienes permiso para modificar este usuario.");

            usuario.Nombre = !string.IsNullOrEmpty(request.Nombre) ? request.Nombre : usuario.Nombre;
            usuario.Email = !string.IsNullOrEmpty(request.Email) ? request.Email : usuario.Email;

            if (!string.IsNullOrEmpty(request.Password))
            {
                using (var hmac = new HMACSHA512())
                {
                    usuario.PasswordSalt = Convert.ToBase64String(hmac.Key);
                    usuario.PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
                }
            }

            await _usuarioService.UpdateAsync(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            var userClaimId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "0");
            if (userClaimId != id && !User.IsInRole("Admin"))
                return Forbid("No tienes permiso para eliminar este usuario.");

            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult GetAdminData()
        {
            return Ok("Este es un endpoint solo para administradores.");
        }
    }

    public class UpdateUserRequest
    {
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
