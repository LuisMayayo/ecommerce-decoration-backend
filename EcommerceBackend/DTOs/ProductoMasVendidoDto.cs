namespace EcommerceBackend.DTOs
{
    public class ProductoMasVendidoDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal TotalVendido { get; set; }
        public string? UrlImagen { get; set; }
    }
}