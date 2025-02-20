using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceBackend.Data;
using EcommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Repositories
{
    public class DetallePedidoRepository : IDetallePedidoRepository
    {
        private readonly EcommerceDbContext _context;

        public DetallePedidoRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _context.DetallesPedido.Where(d => d.PedidoId == pedidoId).ToListAsync();
        }

        public async Task AddAsync(DetallePedido detallePedido)
        {
            await _context.DetallesPedido.AddAsync(detallePedido);
            await _context.SaveChangesAsync();
        }
    }
}
