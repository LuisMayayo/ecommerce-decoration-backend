using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Repositories
{
    public interface IDetallePedidoRepository
    {
        Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId);
        Task AddAsync(DetallePedido detallePedido);
    }
}
