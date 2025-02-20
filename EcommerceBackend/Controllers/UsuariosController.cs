using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly JwtService _jwtService;

        public UsuarioController(IUsuarioService usuarioService, JwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        // Registro de usuario con validación de email y contraseña segura
        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register([FromBody] RegisterRequest request)
        {
            // Validar formato de email
            var emailPattern = @"^[^\s@]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(request.Email, emailPattern))
            {
                return BadRequest("El correo electrónico no es válido.");
            }

            // Validar contraseña segura (mínimo 8 caracteres, una mayúscula, un número y un carácter especial)
            var passwordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!Regex.IsMatch(request.Password, passwordPattern))
            {
                return BadRequest("La contraseña debe tener al menos 8 caracteres, una mayúscula, un número y un carácter especial.");
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

        // Login del usuario con generación de JWT
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
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

            // Generar Token JWT
            var token = _jwtService.GenerateToken(usuario.Id, usuario.Email, usuario.EsAdmin);

            return Ok(new { Token = token });
        }

        // Obtener un usuario por ID (Protegido con JWT)
        [Authorize]
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

        // Endpoint solo para administradores
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult GetAdminData()
        {
            return Ok("Este es un endpoint solo para administradores.");
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
