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
        return productos.Select(p => p.ToDTO()).ToList(); // Conversión a DTO
    }

    public async Task<ProductoDTO?> GetByIdAsync(int id)
    {
        var producto = await _productoRepository.GetByIdAsync(id);
        return producto?.ToDTO();
    }

    public async Task<List<ProductoDTO>> GetByCategoriaIdAsync(int categoriaId)
    {
        var productos = await _productoRepository.GetByCategoriaIdAsync(categoriaId);
        return productos.Select(p => p.ToDTO()).ToList(); // Conversión a DTO
    }

    public async Task AddAsync(Producto producto)
    {
        await _productoRepository.AddAsync(producto);
    }

    public async Task UpdateAsync(Producto producto)
    {
        await _productoRepository.UpdateAsync(producto);
    }

    public async Task DeleteAsync(int id)
    {
        await _productoRepository.DeleteAsync(id);
    }
}
