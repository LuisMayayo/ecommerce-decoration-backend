using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Repositories
{
    public interface IProveedorRepository
    {
        Task<List<Proveedor>> GetAllAsync();
        Task<Proveedor> GetByIdAsync(int id);
        Task AddAsync(Proveedor proveedor);
        Task UpdateAsync(Proveedor proveedor);
        Task DeleteAsync(int id);
        Task<bool> ExistsNifAsync(string nif, int? excludeId = null);
        Task<bool> ExistsEmailAsync(string email, int? excludeId = null);
        Task<List<Producto>> GetProductosByProveedorIdAsync(int proveedorId);
    }
}