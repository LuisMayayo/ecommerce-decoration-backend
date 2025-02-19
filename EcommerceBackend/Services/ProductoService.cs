using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<List<ProductoDTO>> GetAllAsync()
    {
        var productos = await _productoRepository.GetAllAsync();

        // Transformar los productos a ProductoDTO
        return productos.Select(p => new ProductoDTO
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Precio = p.Precio,
            Descripcion = p.Descripcion,
            UrlImagen = p.UrlImagen,
            CategoriaId = p.CategoriaId
        }).ToList();
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

    public async Task<List<ProductoDTO>> GetByCategoriaIdAsync(int categoriaId)
    {
        var productos = await _productoRepository.GetByCategoriaIdAsync(categoriaId);
        return productos.Select(p => new ProductoDTO
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Precio = p.Precio,
            Descripcion = p.Descripcion,
            UrlImagen = p.UrlImagen,
            CategoriaId = p.CategoriaId
        }).ToList();
    }
}
