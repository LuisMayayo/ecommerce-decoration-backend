public class DetallePedidoService : IDetallePedidoService
{
    private readonly IDetallePedidoRepository _detallePedidoRepository;

    public DetallePedidoService(IDetallePedidoRepository detallePedidoRepository)
    {
        _detallePedidoRepository = detallePedidoRepository;
    }

    public async Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId)
    {
        return await _detallePedidoRepository.GetByPedidoIdAsync(pedidoId);
    }

    public async Task AddAsync(DetallePedido detallePedido)
    {
        await _detallePedidoRepository.AddAsync(detallePedido);
    }
}
