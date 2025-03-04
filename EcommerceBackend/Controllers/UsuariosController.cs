using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using EcommerceBackend.Models;
using EcommerceBackend.DTOs;
using EcommerceBackend.Services;
using System.Security.Claims;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            return Ok(usuario);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            Console.WriteLine("🔍 Verificando autenticación...");

            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                Console.WriteLine("✅ Usuario autenticado.");
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"🔹 Claim: {claim.Type} - {claim.Value}");
                }
            }
            else
            {
                Console.WriteLine("❌ No se encontró un usuario autenticado.");
                return StatusCode(StatusCodes.Status401Unauthorized, "No se encontró un usuario autenticado.");
            }
            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var userRole = User.FindFirst("role")?.Value ?? User.FindFirst(ClaimTypes.Role)?.Value ?? "No definido";

            Console.WriteLine($"🔍 Usuario autenticado ID: {userClaimId}, Rol: {userRole}");

            if (string.IsNullOrEmpty(userClaimId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "No se encontró un usuario autenticado.");
            }

            if (int.Parse(userClaimId) != id && userRole != "Admin")
            {
                return StatusCode(StatusCodes.Status403Forbidden, "No tienes permiso para modificar este usuario.");
            }

            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            usuario.Nombre = !string.IsNullOrEmpty(request.Nombre) ? request.Nombre : usuario.Nombre;
            usuario.Email = !string.IsNullOrEmpty(request.Email) ? request.Email : usuario.Email;
            usuario.Telefono = !string.IsNullOrEmpty(request.Telefono) ? request.Telefono : usuario.Telefono;
            usuario.Direccion = !string.IsNullOrEmpty(request.Direccion) ? request.Direccion : usuario.Direccion;

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
            Console.WriteLine("🔍 Verificando autenticación...");

            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                Console.WriteLine("✅ Usuario autenticado.");
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"🔹 Claim: {claim.Type} - {claim.Value}");
                }
            }
            else
            {
                Console.WriteLine("❌ No se encontró un usuario autenticado.");
                return StatusCode(StatusCodes.Status401Unauthorized, "No se encontró un usuario autenticado.");
            }

            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst("role")?.Value ?? User.FindFirst(ClaimTypes.Role)?.Value ?? "No definido";

            Console.WriteLine($"🔍 Usuario autenticado ID: {userClaimId}, Rol: {userRole}");

            if (string.IsNullOrEmpty(userClaimId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "No se encontró un usuario autenticado.");
            }

            if (int.Parse(userClaimId) != id && userRole != "Admin")
            {
                return StatusCode(StatusCodes.Status403Forbidden, "No tienes permiso para eliminar este usuario.");
            }

            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

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

    // DTO para actualizar datos
    public class UpdateUserRequest
    {
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
    }
}
