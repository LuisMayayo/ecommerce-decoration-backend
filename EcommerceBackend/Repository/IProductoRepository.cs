using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Repositories
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetAllAsync();
        Task<Producto> GetByIdAsync(int id);
        Task AddAsync(Producto producto);
        Task UpdateAsync(Producto producto);
        Task DeleteAsync(int id);
        Task<List<Producto>> GetByCategoriaIdAsync(int categoriaId);
        // Nuevo método para buscar productos por nombre
        Task<List<Producto>> SearchByNameAsync(string query);
    }
}
