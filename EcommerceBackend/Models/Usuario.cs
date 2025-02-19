public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
    public bool EsAdmin { get; set; }

    public void Validate()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Nombre))
            throw new ArgumentException("Email y Nombre son requeridos.");
    }
}
