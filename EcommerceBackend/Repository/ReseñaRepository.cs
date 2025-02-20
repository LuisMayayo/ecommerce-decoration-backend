using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceBackend.Data;
using EcommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Repositories
{
    public class ReseñaRepository : IReseñaRepository
    {
        private readonly EcommerceDbContext _context;

        public ReseñaRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Reseña> AddAsync(Reseña reseña)
        {
            await _context.Reseñas.AddAsync(reseña);
            await _context.SaveChangesAsync();
            return reseña;
        }

        public async Task<List<Reseña>> GetByProductoIdAsync(int productoId)
        {
            return await _context.Reseñas.Where(r => r.ProductoId == productoId).ToListAsync();
        }
    }
}
