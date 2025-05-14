namespace EcommerceBackend.DTOs
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string NIF { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PersonaContacto { get; set; } = string.Empty;
    }
}