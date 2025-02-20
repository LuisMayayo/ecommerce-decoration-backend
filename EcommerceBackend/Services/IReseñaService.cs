using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Services
{
    public interface IReseñaService
    {
        Task<Reseña> AddAsync(Reseña reseña);
        Task<List<Reseña>> GetByProductoIdAsync(int productoId);
    }
}
