using EcommerceBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBackend.Services
{
    public interface IPedidoService
    {
        Task<List<Pedido>> GetByUserIdAsync(int userId);
        Task<Pedido> GetByIdAsync(int id);
        Task<Pedido> AddAsync(Pedido pedido);
        Task DeleteAsync(int id);
    }
}
