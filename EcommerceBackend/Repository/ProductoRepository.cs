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
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .AsNoTracking() 
                .ToListAsync();
        }
        
        public async Task<Producto> GetByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .AsNoTracking() 
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task AddAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateAsync(Producto producto)
        {
            _context.ChangeTracker.Clear();
            
            var exists = await _context.Productos
                .AsNoTracking()
                .AnyAsync(p => p.Id == producto.Id);
                
            if (!exists)
            {
                throw new KeyNotFoundException($"Producto con ID {producto.Id} no encontrado");
            }
            
            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<List<Producto>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .AsNoTracking()
                .Where(p => p.CategoriaId == categoriaId)
                .ToListAsync();
        }
    }
}