using EcommerceBackend.Data;
using EcommerceBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return await _context.DetallePedido // 👈 Asegura que el nombre coincide
                .Where(d => d.PedidoId == pedidoId)
                .ToListAsync();
        }

        public async Task<DetallePedido> GetByIdAsync(int id)
        {
            return await _context.DetallePedido.FindAsync(id); // 👈 Aquí también
        }

        public async Task AddAsync(DetallePedido detallePedido)
        {
            _context.DetallePedido.Add(detallePedido); // 👈 Corregido
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var detalle = await _context.DetallePedido.FindAsync(id);
            if (detalle != null)
            {
                _context.DetallePedido.Remove(detalle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
