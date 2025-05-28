namespace EcommerceBackend.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public int? ProveedorId { get; set; }
        public string ProveedorNombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string UrlImagen { get; set; } = string.Empty;
        public string ModeloUrl3D { get; set; } = string.Empty;
    }
}