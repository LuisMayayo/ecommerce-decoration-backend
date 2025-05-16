using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Repositories;
using EcommerceBackend.Extensions;

namespace EcommerceBackend.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _proveedorRepository;
        
        public ProveedorService(IProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }
        
        public async Task<List<Proveedor>> GetAllAsync()
        {
            return await _proveedorRepository.GetAllAsync();
        }
        
        public async Task<Proveedor> GetByIdAsync(int id)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(id);
            if (proveedor == null)
                throw new KeyNotFoundException($"Proveedor con ID {id} no encontrado");
                
            return proveedor;
        }
        
        public async Task<int> AddAsync(Proveedor proveedor)
        {
            proveedor.Validate();
            
            // Verificar que el NIF no está duplicado
            if (await _proveedorRepository.ExistsNifAsync(proveedor.NIF))
                throw new ArgumentException($"Ya existe un proveedor con el NIF {proveedor.NIF}");
                
            // Verificar que el email no está duplicado
            if (await _proveedorRepository.ExistsEmailAsync(proveedor.Email))
                throw new ArgumentException($"Ya existe un proveedor con el email {proveedor.Email}");
                
            await _proveedorRepository.AddAsync(proveedor);
            return proveedor.Id;
        }
        
        public async Task UpdateAsync(Proveedor proveedor)
        {
            var existingProvider = await _proveedorRepository.GetByIdAsync(proveedor.Id);
            if (existingProvider == null)
                throw new KeyNotFoundException($"Proveedor con ID {proveedor.Id} no encontrado");
                
            proveedor.Validate();
            
            // Verificar que el NIF no está duplicado (excluyendo el ID actual)
            if (await _proveedorRepository.ExistsNifAsync(proveedor.NIF, proveedor.Id))
                throw new ArgumentException($"Ya existe un proveedor con el NIF {proveedor.NIF}");
                
            // Verificar que el email no está duplicado (excluyendo el ID actual)
            if (await _proveedorRepository.ExistsEmailAsync(proveedor.Email, proveedor.Id))
                throw new ArgumentException($"Ya existe un proveedor con el email {proveedor.Email}");
                
            await _proveedorRepository.UpdateAsync(proveedor);
        }
        
        public async Task DeleteAsync(int id)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(id);
            if (proveedor == null)
                throw new KeyNotFoundException($"Proveedor con ID {id} no encontrado");
                
            // Verificar si hay productos asociados a este proveedor
            var productos = await _proveedorRepository.GetProductosByProveedorIdAsync(id);
            if (productos.Count > 0)
                throw new InvalidOperationException($"No se puede eliminar el proveedor porque tiene {productos.Count} productos asociados");
                
            await _proveedorRepository.DeleteAsync(id);
        }
        
        public async Task<List<Producto>> GetProductosByProveedorIdAsync(int proveedorId)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(proveedorId);
            if (proveedor == null)
                throw new KeyNotFoundException($"Proveedor con ID {proveedorId} no encontrado");
                
            return await _proveedorRepository.GetProductosByProveedorIdAsync(proveedorId);
        }
    }
}