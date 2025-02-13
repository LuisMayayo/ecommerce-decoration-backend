using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

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

        // Crear un nuevo usuario
        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register([FromBody] Usuario usuario)
        {
            // Verificar si el usuario ya existe
            var existingUser = await _usuarioService.GetByEmailAsync(usuario.Email);
            if (existingUser != null)
            {
                return Conflict("El usuario ya existe con ese correo.");
            }

            // Crear el PasswordHash y PasswordSalt
            using (var hmac = new HMACSHA512())
            {
                usuario.PasswordSalt = Convert.ToBase64String(hmac.Key);  // Guardar la sal
                usuario.PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(usuario.PasswordHash))); // Guardar el hash
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

        // Login del usuario (autenticación)
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
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
                if (computedHash != usuario.PasswordHash)
                {
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }
            }

            return Ok(usuario);
        }
    }

    // Clase para la solicitud de login
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
