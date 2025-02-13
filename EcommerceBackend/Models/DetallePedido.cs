public class DetallePedido
{
    public int Id { get; set; }
    public int PedidoId { get; set; }  // Relación con Pedido
    public int ProductoId { get; set; }  // Relación con Producto
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
