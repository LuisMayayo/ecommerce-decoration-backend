public interface IReseñaRepository
{
    Task<Reseña> AddAsync(Reseña reseña);
    Task<List<Reseña>> GetByProductoIdAsync(int productoId);  // Obtener reseñas por producto
}
