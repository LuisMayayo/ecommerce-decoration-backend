using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EcommerceBackend.Models
{
    [Table("Proveedor")]
    public class Proveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        public string Direccion { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(15)]
        public string NIF { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? PersonaContacto { get; set; }
        
        // Propiedad de navegación para los productos asociados al proveedor
        public ICollection<Producto>? Productos { get; set; }
    }
}