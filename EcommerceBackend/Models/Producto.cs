using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceBackend.Models
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        public decimal Precio { get; set; }
        
        [Required]
        public int CategoriaId { get; set; }
        
        public int? ProveedorId { get; set; }
        
        public string? UrlImagen { get; set; }
        
        public string? Descripcion { get; set; }
    
        public string? ModeloUrl3D { get; set; }
        public Categoria? Categoria { get; set; }
        
        public Proveedor? Proveedor { get; set; }
    }
}