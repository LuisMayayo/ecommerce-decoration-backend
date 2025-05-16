namespace EcommerceBackend.DTOs
{
    public class VentasPorCategoriaDto
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public double Porcentaje { get; set; }
    }
}
