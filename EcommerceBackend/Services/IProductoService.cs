using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductoService
{
    Task<List<ProductoDTO>> GetAllAsync();
    Task<ProductoDTO?> GetByIdAsync(int id);
    Task<List<ProductoDTO>> GetByCategoriaIdAsync(int categoriaId);
    Task AddAsync(Producto producto);
    Task UpdateAsync(Producto producto);
    Task DeleteAsync(int id);
}
