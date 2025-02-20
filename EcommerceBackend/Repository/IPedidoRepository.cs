using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Repositories
{
    public interface IPedidoRepository
    {
        Task<Pedido> AddAsync(Pedido pedido);
        Task<Pedido> GetByIdAsync(int id);
        Task<List<Pedido>> GetByUserIdAsync(int userId);
    }
}
