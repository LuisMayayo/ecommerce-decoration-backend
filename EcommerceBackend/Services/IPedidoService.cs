using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Services
{
    public interface IPedidoService
    {
        Task<Pedido> AddAsync(Pedido pedido);
        Task<Pedido> GetByIdAsync(int id);
        Task<List<Pedido>> GetByUserIdAsync(int userId);
    }
}
