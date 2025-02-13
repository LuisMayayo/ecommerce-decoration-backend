public interface IDetallePedidoService
{
    Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId);
    Task AddAsync(DetallePedido detallePedido);
}
