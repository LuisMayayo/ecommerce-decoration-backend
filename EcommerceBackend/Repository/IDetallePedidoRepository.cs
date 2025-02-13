public interface IDetallePedidoRepository
{
    Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId);
    Task AddAsync(DetallePedido detallePedido);
}
