public class Reseña
{
    public int Id { get; set; }
    public int ProductoId { get; set; }  // Relación con Producto
    public int UsuarioId { get; set; }  // Relación con Usuario
    public string Comentario { get; set; } = string.Empty;
    public int Calificacion { get; set; }  // Calificación de 1 a 5
}
