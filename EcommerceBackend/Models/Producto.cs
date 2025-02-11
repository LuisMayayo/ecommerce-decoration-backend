using System.ComponentModel.DataAnnotations;

public class Producto
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    [Required]
    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}
