using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Services
{
    public interface IDetallePedidoService
    {
        Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId);
        Task AddAsync(DetallePedido detallePedido);
    }
}
