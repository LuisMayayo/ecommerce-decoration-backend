using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using EcommerceBackend.Models;
using EcommerceBackend.DTOs;
using EcommerceBackend.Services;
using System.IdentityModel.Tokens.Jwt;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly JwtService _jwtService;

        public AuthController(IUsuarioService usuarioService, JwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UsuarioDto>> Register([FromBody] RegisterRequest request)
        {
            var emailPattern = @"^[^\s@]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(request.Email, emailPattern))
                return BadRequest("El correo electrónico no es válido.");

            var passwordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!Regex.IsMatch(request.Password, passwordPattern))
                return BadRequest("La contraseña debe tener al menos 8 caracteres, una mayúscula, un número y un carácter especial.");

            var existingUser = await _usuarioService.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return Conflict("El usuario ya existe con ese correo.");

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Email = request.Email,
                FechaRegistro = DateTime.UtcNow
            };

            using (var hmac = new HMACSHA512())
            {
                usuario.PasswordSalt = Convert.ToBase64String(hmac.Key);
                usuario.PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
            }

            await _usuarioService.AddAsync(usuario);

            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                EsAdmin = usuario.EsAdmin
            };

            return CreatedAtAction(nameof(UsuarioController.GetById), "Usuario", new { id = usuario.Id }, usuarioDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
        {
            var usuario = await _usuarioService.GetByEmailAsync(request.Email);
            if (usuario == null)
                return Unauthorized("Usuario o contraseña incorrectos.");

            using (var hmac = new HMACSHA512(Convert.FromBase64String(usuario.PasswordSalt)))
            {
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
                if (computedHash != usuario.PasswordHash)
                    return Unauthorized("Usuario o contraseña incorrectos.");
            }

            var token = _jwtService.GenerateToken(usuario.Id, usuario.Email, usuario.EsAdmin);
            return Ok(new { Token = token });
        }
    }

    public class RegisterRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
