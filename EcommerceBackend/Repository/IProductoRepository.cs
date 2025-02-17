using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductoRepository
{
    Task<List<Producto>> GetAllAsync();
    Task<Producto?> GetByIdAsync(int id);
    Task AddAsync(Producto producto);
    Task UpdateAsync(Producto producto);
    Task<bool> DeleteAsync(int id);
    
    // Método para obtener productos filtrados por categoriaId
    Task<List<Producto>> GetByCategoriaIdAsync(int categoriaId);
}
