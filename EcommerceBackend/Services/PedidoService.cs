using EcommerceBackend.Models;
using EcommerceBackend.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBackend.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<List<Pedido>> GetByUserIdAsync(int userId)
        {
            return await _pedidoRepository.GetByUserIdAsync(userId);
        }

        public async Task<Pedido> GetByIdAsync(int id)
        {
            return await _pedidoRepository.GetByIdAsync(id);
        }

        public async Task<Pedido> AddAsync(Pedido pedido)
        {
            return await _pedidoRepository.AddAsync(pedido);
        }

        public async Task DeleteAsync(int id)
        {
            await _pedidoRepository.DeleteAsync(id);
        }
    }
}
