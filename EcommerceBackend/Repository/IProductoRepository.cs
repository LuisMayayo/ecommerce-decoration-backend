public interface IProductoRepository
{
    Task<List<Producto>> GetAllAsync();  // Método para obtener todos los productos
    Task<Producto?> GetByIdAsync(int id);  // Método para obtener un producto por su ID
    Task AddAsync(Producto producto);  // Método para agregar un producto
    Task UpdateAsync(Producto producto);  // Método para actualizar un producto
    Task<bool> DeleteAsync(int id);  // Método para eliminar un producto por ID

    // Método para obtener productos por categoría
    Task<List<Producto>> GetByCategoriaIdAsync(int categoriaId);
}
