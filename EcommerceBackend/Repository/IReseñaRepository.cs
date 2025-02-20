using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Repositories
{
    public interface IReseñaRepository
    {
        Task<Reseña> AddAsync(Reseña reseña);
        Task<List<Reseña>> GetByProductoIdAsync(int productoId);
    }
}
