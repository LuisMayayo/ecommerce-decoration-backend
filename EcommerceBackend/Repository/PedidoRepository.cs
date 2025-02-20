using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceBackend.Data;
using EcommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly EcommerceDbContext _context;

        public PedidoRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido> AddAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> GetByIdAsync(int id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task<List<Pedido>> GetByUserIdAsync(int userId)
        {
            return await _context.Pedidos.Where(p => p.UsuarioId == userId).ToListAsync();
        }
    }
}
