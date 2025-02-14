public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int CategoriaId { get; set; }
    public string UrlImagen { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public void Validate()
    {
        if (Precio < 0)
            throw new ArgumentException("El precio no puede ser negativo.");
        if (string.IsNullOrEmpty(Nombre))
            throw new ArgumentException("El nombre es requerido.");
    }
}
