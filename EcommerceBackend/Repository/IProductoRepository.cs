using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductoRepository
{
    /// <summary>
    /// Obtiene todos los productos.
    /// </summary>
    /// <returns>Lista de productos.</returns>
    Task<List<Producto>> GetAllAsync();

    /// <summary>
    /// Obtiene un producto por su ID.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <returns>Producto encontrado o null si no existe.</returns>
    Task<Producto?> GetByIdAsync(int id);

    /// <summary>
    /// Obtiene todos los productos de una categoría específica.
    /// </summary>
    /// <param name="categoriaId">ID de la categoría.</param>
    /// <returns>Lista de productos de la categoría.</returns>
    Task<List<Producto>> GetByCategoriaIdAsync(int categoriaId);

    /// <summary>
    /// Agrega un nuevo producto a la base de datos.
    /// </summary>
    /// <param name="producto">Objeto Producto a agregar.</param>
    /// <returns>True si la operación fue exitosa.</returns>
    Task<bool> AddAsync(Producto producto);

    /// <summary>
    /// Actualiza un producto existente.
    /// </summary>
    /// <param name="producto">Objeto Producto con los nuevos valores.</param>
    /// <returns>True si la operación fue exitosa.</returns>
    Task<bool> UpdateAsync(Producto producto);

    /// <summary>
    /// Elimina un producto por su ID.
    /// </summary>
    /// <param name="id">ID del producto a eliminar.</param>
    /// <returns>True si el producto fue eliminado correctamente.</returns>
    Task<bool> DeleteAsync(int id);
}
