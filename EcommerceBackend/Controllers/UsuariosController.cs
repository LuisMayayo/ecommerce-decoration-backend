using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions; // Importante para usar Regex

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

        // Registro de usuario
        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register([FromBody] RegisterRequest request)
        {
            // Validar formato de email
            // Patrón simple para correos: "algo@dominio.ext"
            var emailPattern = @"^[^\s@]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(request.Email, emailPattern))
            {
                return BadRequest("El correo electrónico no es válido.");
            }

            // Verificar si el usuario ya existe
            var existingUser = await _usuarioService.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict("El usuario ya existe con ese correo.");
            }

            // Crear un nuevo objeto Usuario con los datos recibidos
            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Email = request.Email,
                FechaRegistro = DateTime.Now
            };

            // Generar el PasswordHash y PasswordSalt usando la contraseña en claro
            using (var hmac = new HMACSHA512())
            {
                usuario.PasswordSalt = Convert.ToBase64String(hmac.Key);
                usuario.PasswordHash = Convert.ToBase64String(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password))
                );
            }

            // Registrar el usuario
            await _usuarioService.AddAsync(usuario);

            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        // Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            return Ok(usuario);
        }

        // Login del usuario
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] LoginRequest request)
        {
            var usuario = await _usuarioService.GetByEmailAsync(request.Email);
            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            using (var hmac = new HMACSHA512(Convert.FromBase64String(usuario.PasswordSalt)))
            {
                var computedHash = Convert.ToBase64String(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password))
                );
                if (computedHash != usuario.PasswordHash)
                {
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }
            }

            return Ok(usuario);
        }
    }

    // Objeto para la solicitud de registro
    public class RegisterRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Objeto para la solicitud de login
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
