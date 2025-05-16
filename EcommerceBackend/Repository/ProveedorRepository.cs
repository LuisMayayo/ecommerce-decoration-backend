using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EcommerceBackend.Data;
using EcommerceBackend.Models;

namespace EcommerceBackend.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly EcommerceDbContext _context;

        public ProveedorRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proveedor>> GetAllAsync()
        {
            return await _context.Proveedores.ToListAsync();
        }

        public async Task<Proveedor> GetByIdAsync(int id)
        {
            return await _context.Proveedores
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Proveedor proveedor)
        {
            await _context.Proveedores.AddAsync(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Proveedor proveedor)
        {
            // Desconectar cualquier entidad existente con el mismo ID
            var existingProveedor = await _context.Proveedores.FindAsync(proveedor.Id);
            if (existingProveedor != null)
            {
                _context.Entry(existingProveedor).State = EntityState.Detached;
            }

            // Ahora marca la nueva entidad como modificada
            _context.Entry(proveedor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsNifAsync(string nif, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return await _context.Proveedores.AnyAsync(p => p.NIF == nif && p.Id != excludeId);

            return await _context.Proveedores.AnyAsync(p => p.NIF == nif);
        }

        public async Task<bool> ExistsEmailAsync(string email, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return await _context.Proveedores.AnyAsync(p => p.Email == email && p.Id != excludeId);

            return await _context.Proveedores.AnyAsync(p => p.Email == email);
        }

        public async Task<List<Producto>> GetProductosByProveedorIdAsync(int proveedorId)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.ProveedorId == proveedorId)
                .ToListAsync();
        }
    }
}