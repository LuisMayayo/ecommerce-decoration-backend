public interface IPedidoService
{
    Task<Pedido> AddAsync(Pedido pedido);
    Task<Pedido> GetByIdAsync(int id);
    Task<List<Pedido>> GetByUserIdAsync(int userId);  // Obtener pedidos por usuario
}
