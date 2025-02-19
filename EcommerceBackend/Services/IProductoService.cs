public interface IProductoService
{
    Task<List<ProductoDTO>> GetAllAsync();  // Devolver un DTO
    Task<Producto?> GetByIdAsync(int id);
    Task AddAsync(Producto producto);
    Task UpdateAsync(Producto producto);
    Task DeleteAsync(int id);
    Task<List<ProductoDTO>> GetByCategoriaIdAsync(int categoriaId);  // Usar DTO también
}
