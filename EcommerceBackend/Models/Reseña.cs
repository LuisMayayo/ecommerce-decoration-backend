public class Reseña
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public int UsuarioId { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public int Calificacion { get; set; }

    public void Validate()
    {
        if (Calificacion < 1 || Calificacion > 5)
            throw new ArgumentException("La calificación debe ser entre 1 y 5.");
    }
}
