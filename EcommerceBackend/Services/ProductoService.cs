using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Repositories;
using EcommerceBackend.Extensions;

namespace EcommerceBackend.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        
        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }
        
        public async Task<List<Producto>> GetAllAsync()
        {
            return await _productoRepository.GetAllAsync();
        }
        
        public async Task<Producto> GetByIdAsync(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");
                
            return producto;
        }
        
        public async Task AddAsync(Producto producto)
        {
            producto.Validate();
            await _productoRepository.AddAsync(producto);
        }
        
        public async Task UpdateAsync(Producto producto)
        {
            var existingProduct = await _productoRepository.GetByIdAsync(producto.Id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Producto con ID {producto.Id} no encontrado");
                
            producto.Validate();
            await _productoRepository.UpdateAsync(producto);
        }
        
        public async Task DeleteAsync(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");
                
            await _productoRepository.DeleteAsync(id);
        }
        
        public async Task<List<Producto>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _productoRepository.GetByCategoriaIdAsync(categoriaId);
        }
    }
}