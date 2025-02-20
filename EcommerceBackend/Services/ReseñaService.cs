using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Repositories;

namespace EcommerceBackend.Services
{
    public class ReseñaService : IReseñaService
    {
        private readonly IReseñaRepository _reseñaRepository;

        public ReseñaService(IReseñaRepository reseñaRepository)
        {
            _reseñaRepository = reseñaRepository;
        }

        public async Task<Reseña> AddAsync(Reseña reseña)
        {
            return await _reseñaRepository.AddAsync(reseña);
        }

        public async Task<List<Reseña>> GetByProductoIdAsync(int productoId)
        {
            return await _reseñaRepository.GetByProductoIdAsync(productoId);
        }
    }
}
