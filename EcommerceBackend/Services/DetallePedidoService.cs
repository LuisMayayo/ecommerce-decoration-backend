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
        // Validaciones adicionales si es necesario
        if (detallePedido.Cantidad <= 0)
        {
            throw new ArgumentException("La cantidad debe ser mayor que 0.");
        }

        await _detallePedidoRepository.AddAsync(detallePedido);
    }
}
