using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<Producto>> GetAll()
    {
        return await _productoRepository.GetAll();
    }

    public async Task<Producto> GetById(int id)
    {
        return await _productoRepository.GetById(id);
    }

    public async Task Add(Producto producto)
    {
        await _productoRepository.Add(producto);
    }

    public async Task Update(Producto producto)
    {
        await _productoRepository.Update(producto);
    }

    public async Task Delete(int id)
    {
        await _productoRepository.Delete(id);
    }
}
