public class Pedido
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }  // Relación con Usuario
    public DateTime FechaPedido { get; set; }
    public decimal Total { get; set; }

    // El campo Estado se ha eliminado.
}
