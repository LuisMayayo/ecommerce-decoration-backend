using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceBackend.Models
{
    [Table("DetallePedido")]
    public class DetallePedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public Pedido? Pedido { get; set; }
        public Producto? Producto { get; set; }
    }
}
