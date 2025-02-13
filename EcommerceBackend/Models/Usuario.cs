public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;  // Hash de la contraseña
    public string PasswordSalt { get; set; } = string.Empty;  // Sal de la contraseña
    public DateTime FechaRegistro { get; set; }
}
