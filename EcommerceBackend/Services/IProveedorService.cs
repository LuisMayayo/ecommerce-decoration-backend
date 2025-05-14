using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Services
{
    public interface IProveedorService
    {
        Task<List<Proveedor>> GetAllAsync();
        Task<Proveedor> GetByIdAsync(int id);
        Task<int> AddAsync(Proveedor proveedor);
        Task UpdateAsync(Proveedor proveedor);
        Task DeleteAsync(int id);
        Task<List<Producto>> GetProductosByProveedorIdAsync(int proveedorId);
    }
}