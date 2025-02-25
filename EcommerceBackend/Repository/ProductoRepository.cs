using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EcommerceBackend.Data;
using EcommerceBackend.Models;

namespace EcommerceBackend.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly EcommerceDbContext _context;

        public ProductoRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> GetAllAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Producto> GetByIdAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task AddAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await GetByIdAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Producto>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _context.Productos.Where(p => p.CategoriaId == categoriaId).ToListAsync();
        }

        // Método modificado para filtrar por palabras en el nombre del producto
        public async Task<List<Producto>> SearchByNameAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return await GetAllAsync();
            }

            var words = query.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
                             .Select(word => word.ToLower())
                             .ToArray();

            return await _context.Productos
                .Where(p => words.Any(word => p.Nombre.ToLower().Contains(word)))
                .ToListAsync();
        }
    }
}
