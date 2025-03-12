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

        /// <summary>
        /// Obtener detalles de un pedido por su ID.
        /// Incluye información del producto y la categoría.
        /// </summary>
        public async Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _context.DetallesPedido
                .Include(d => d.Producto)
                    .ThenInclude(p => p.Categoria) 
                .Where(d => d.PedidoId == pedidoId)
                .ToListAsync();
        }

        public async Task<DetallePedido> GetByIdAsync(int id)
        {
            return await _context.DetallesPedido
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(DetallePedido detallePedido)
        {
            _context.DetallesPedido.Add(detallePedido);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Eliminar un detalle de pedido por su ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var detalle = await _context.DetallesPedido.FindAsync(id); 
            if (detalle != null)
            {
                _context.DetallesPedido.Remove(detalle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
