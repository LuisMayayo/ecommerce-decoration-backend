using EcommerceBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBackend.Repositories
{
    public interface IReseñaRepository
    {
        Task<List<Reseña>> GetByProductoIdAsync(int productoId);
        Task AddAsync(Reseña reseña);
        Task DeleteAsync(int id);
        Task<double> GetPromedioCalificacionAsync(int productoId);
    }
}
