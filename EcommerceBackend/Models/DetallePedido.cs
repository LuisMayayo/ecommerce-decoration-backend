public class DetallePedido
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }

    public void Validate()
    {
        if (Cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que 0.");
        if (PrecioUnitario <= 0)
            throw new ArgumentException("El precio unitario debe ser mayor que 0.");
    }
}
