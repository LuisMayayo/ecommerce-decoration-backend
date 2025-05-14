using System;
using EcommerceBackend.Models;
using EcommerceBackend.DTOs;

namespace EcommerceBackend.Extensions
{
    public static class ProveedorExtensions
    {
        public static void Validate(this Proveedor proveedor)
        {
            if (string.IsNullOrWhiteSpace(proveedor.Nombre))
                throw new ArgumentException("El nombre del proveedor no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(proveedor.Direccion))
                throw new ArgumentException("La dirección del proveedor no puede estar vacía.");
            if (string.IsNullOrWhiteSpace(proveedor.NIF))
                throw new ArgumentException("El NIF del proveedor no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(proveedor.Telefono))
                throw new ArgumentException("El teléfono del proveedor no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(proveedor.Email))
                throw new ArgumentException("El email del proveedor no puede estar vacío.");
            if (!proveedor.Email.Contains("@"))
                throw new ArgumentException("El email del proveedor debe ser válido.");
        }
        
        public static ProveedorDto ToDto(this Proveedor proveedor)
        {
            return new ProveedorDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Direccion = proveedor.Direccion,
                NIF = proveedor.NIF,
                Telefono = proveedor.Telefono,
                Email = proveedor.Email,
                PersonaContacto = proveedor.PersonaContacto ?? string.Empty
            };
        }
    }
}