using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<List<Producto>> GetAllAsync()
    {
        return await _productoRepository.GetAllAsync();
    }

    public async Task<Producto?> GetByIdAsync(int id)
    {
        return await _productoRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Producto producto)
    {
        if (producto.Precio <= 0)
        {
            throw new ArgumentException("El precio del producto debe ser mayor que 0.");
        }
        await _productoRepository.AddAsync(producto);
    }

    public async Task UpdateAsync(Producto producto)
    {
        if (producto.Precio <= 0)
        {
            throw new ArgumentException("El precio del producto debe ser mayor que 0.");
        }
        await _productoRepository.UpdateAsync(producto);
    }

    public async Task DeleteAsync(int id)
    {
        await _productoRepository.DeleteAsync(id);
    }
    public async Task<List<Producto>> GetByCategoriaIdAsync(int categoriaId)
{
    return await _productoRepository.GetByCategoriaIdAsync(categoriaId);
}

}
