using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using EcommerceBackend.Models;
using EcommerceBackend.DTOs;
using EcommerceBackend.Services;
using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Auth;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUsuarioService usuarioService, 
            JwtService jwtService, 
            IEmailService emailService,
            IWebHostEnvironment environment,
            ILogger<AuthController> logger)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
            _emailService = emailService;
            _environment = environment;
            _logger = logger;
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
                FechaRegistro = DateTime.UtcNow,
                EsAdmin = request.EsAdmin,
                Telefono = request.Telefono ?? string.Empty,
                Direccion = request.Direccion ?? string.Empty
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
                EsAdmin = usuario.EsAdmin,
                Telefono = usuario.Telefono,
                Direccion = usuario.Direccion
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
            return Ok(new { Token = token, EsAdmin = usuario.EsAdmin });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                _logger.LogInformation($"Solicitud de reset de contraseña para: {request.Email}");

                // Validar entrada
                if (string.IsNullOrEmpty(request.Email))
                {
                    return BadRequest("Email es requerido.");
                }

                var emailPattern = @"^[^\s@]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(request.Email, emailPattern))
                {
                    return BadRequest("Formato de email inválido.");
                }

                var usuario = await _usuarioService.GetByEmailAsync(request.Email);
                
                // Siempre devolver la misma respuesta por seguridad
                const string successMessage = "Si el correo existe en nuestro sistema, se enviará un enlace para restablecer la contraseña.";
                
                if (usuario == null)
                {
                    // Log para debugging pero no revelar al usuario
                    _logger.LogWarning($"Intento de reset para email no registrado: {request.Email}");
                    return Ok(successMessage);
                }

                try
                {
                    // Generate a unique reset token
                    var resetToken = Guid.NewGuid().ToString();
                    usuario.ResetToken = resetToken;
                    usuario.ResetTokenExpiration = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour

                    _logger.LogInformation($"Token de reset generado para usuario {usuario.Id}");

                    // Save changes to the database
                    await _usuarioService.UpdateAsync(usuario);

                    // Crear enlace de reset (usar la URL correcta del frontend)
                    var resetLink = _environment.IsDevelopment() 
                        ? $"http://localhost:5173/reset-password?token={resetToken}"
                        : $"https://lm-decoraciones.retocsv.es/reset-password?token={resetToken}";
                    
                    _logger.LogInformation($"Enlace de reset generado: {resetLink}");
                    
                    // Crear cuerpo del email en HTML
                    var htmlBody = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                            <h2 style='color: #333;'>Restablecer contraseña - LM Decoraciones</h2>
                            <p>Hola <strong>{usuario.Nombre}</strong>,</p>
                            <p>Hemos recibido una solicitud para restablecer tu contraseña.</p>
                            <p>Haz clic en el siguiente botón para restablecer tu contraseña:</p>
                            <div style='text-align: center; margin: 30px 0;'>
                                <a href='{resetLink}' 
                                   style='background-color: #ffc107; 
                                          color: #000; 
                                          padding: 12px 25px; 
                                          text-decoration: none; 
                                          border-radius: 5px; 
                                          display: inline-block;
                                          font-weight: bold;'>
                                    Restablecer Contraseña
                                </a>
                            </div>
                            <p style='color: #666; font-size: 14px;'>
                                Si no puedes hacer clic en el botón, copia y pega este enlace en tu navegador:<br/>
                                <a href='{resetLink}'>{resetLink}</a>
                            </p>
                            <p style='color: #666; font-size: 14px;'>
                                Este enlace expirará en 1 hora por motivos de seguridad.
                            </p>
                            <p style='color: #666; font-size: 14px;'>
                                Si no solicitaste este cambio, puedes ignorar este mensaje de forma segura.
                            </p>
                            <hr style='margin: 30px 0; border: none; border-top: 1px solid #eee;'/>
                            <p style='color: #999; font-size: 12px; text-align: center;'>
                                Este email fue enviado por LM Decoraciones<br/>
                                No respondas a este email, es generado automáticamente.
                            </p>
                        </div>";

                    var subject = "Restablecer contraseña - LM Decoraciones";
                    
                    _logger.LogInformation($"Enviando email de reset a: {usuario.Email}");
                    await _emailService.SendEmailAsync(usuario.Email, subject, htmlBody);
                    
                    _logger.LogInformation($"Email de reset enviado exitosamente a: {request.Email}");
                }
                catch (Exception emailEx)
                {
                    // Log del error pero no fallar la operación por seguridad
                    _logger.LogError(emailEx, $"Error enviando email de reset: {emailEx.Message}");
                    
                    // En desarrollo, puedes ser más específico
                    if (_environment.IsDevelopment())
                    {
                        return StatusCode(500, $"Error interno: No se pudo enviar el email. {emailEx.Message}");
                    }
                    
                    // En producción, devolver error genérico
                    return StatusCode(500, "Error interno del servidor al procesar la solicitud.");
                }

                return Ok(successMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error general en forgot-password: {ex.Message}");
                
                if (_environment.IsDevelopment())
                {
                    return StatusCode(500, $"Error interno del servidor: {ex.Message}");
                }
                
                return StatusCode(500, "Error interno del servidor. Inténtalo más tarde.");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                _logger.LogInformation($"Intento de reset de contraseña con token: {request.Token?.Substring(0, 8)}...");

                // Validate token format
                if (string.IsNullOrEmpty(request.Token))
                    return BadRequest("Token inválido.");

                // Look up user by reset token
                var usuario = await _usuarioService.GetByResetTokenAsync(request.Token);
                if (usuario == null || usuario.ResetTokenExpiration < DateTime.UtcNow)
                {
                    _logger.LogWarning($"Token inválido o expirado: {request.Token}");
                    return BadRequest("Token inválido o expirado.");
                }

                // Validate new password format
                var passwordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
                if (!Regex.IsMatch(request.NewPassword, passwordPattern))
                    return BadRequest("La contraseña debe tener al menos 8 caracteres, una mayúscula, un número y un carácter especial.");

                // Hash the new password
                string newPasswordHash;
                string newPasswordSalt;
                using (var hmac = new HMACSHA512())
                {
                    newPasswordSalt = Convert.ToBase64String(hmac.Key);
                    newPasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.NewPassword)));
                }

                // Update user with new password and clear reset token
                usuario.PasswordHash = newPasswordHash;
                usuario.PasswordSalt = newPasswordSalt;
                usuario.ResetToken = null;
                usuario.ResetTokenExpiration = null;

                await _usuarioService.UpdateAsync(usuario);

                _logger.LogInformation($"Contraseña restablecida exitosamente para usuario: {usuario.Email}");

                return Ok("Contraseña restablecida exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en reset-password: {ex.Message}");
                
                if (_environment.IsDevelopment())
                {
                    return StatusCode(500, $"Error interno: {ex.Message}");
                }
                
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost("google-login")]
        public async Task<ActionResult<object>> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    request.IdToken,
                    new GoogleJsonWebSignature.ValidationSettings()
                );
                if (payload == null)
                    return Unauthorized("Token de Google inválido.");

                var usuario = await _usuarioService.GetByEmailAsync(payload.Email);
                if (usuario == null)
                {
                    usuario = new Usuario
                    {
                        Nombre = payload.Name ?? "Usuario Google",
                        Email = payload.Email,
                        FechaRegistro = DateTime.UtcNow
                    };
                    await _usuarioService.AddAsync(usuario);
                }

                var token = _jwtService.GenerateToken(usuario.Id, usuario.Email, usuario.EsAdmin);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en google-login");
                return Unauthorized("Error al validar token de Google: " + ex.Message);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            return Ok("Cuenta confirmada exitosamente (Simulación).");
        }
    }

    public class RegisterRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EsAdmin { get; set; } = false;
        public string? Telefono { get; set; } = string.Empty;
        public string? Direccion { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class GoogleLoginRequest
    {
        public string IdToken { get; set; } = string.Empty;
    }
}