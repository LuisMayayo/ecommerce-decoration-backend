using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceBackend.DTOs;
using EcommerceBackend.Interfaces;
using EcommerceBackend.Models;
using EcommerceBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly EcommerceDbContext _context;

        public DashboardRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalUsuariosAsync()
        {
            return await _context.Usuarios.CountAsync();
        }

        public async Task<int> GetTotalProductosAsync()
        {
            return await _context.Productos.CountAsync();
        }

        public async Task<int> GetTotalPedidosAsync()
        {
            return await _context.Pedidos.CountAsync();
        }

        public async Task<decimal> GetVentasTotalesAsync()
        {
            return await _context.Pedidos.SumAsync(p => p.Total);
        }

        public async Task<decimal> GetVentasHoyAsync()
        {
            var hoy = DateTime.Today;
            return await _context.Pedidos
                .Where(p => p.FechaPedido.Date == hoy)
                .SumAsync(p => p.Total);
        }

        public async Task<List<ProductoMasVendidoDto>> GetProductosMasVendidosAsync(int limite = 5)
        {
            // Obtenemos los productos más vendidos basados en los detalles de pedido
            var productosMasVendidos = await _context.DetallesPedido
                .GroupBy(d => d.ProductoId)
                .Select(g => new
                {
                    ProductoId = g.Key,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    TotalVendido = g.Sum(d => d.PrecioUnitario * d.Cantidad)
                })
                .OrderByDescending(x => x.CantidadVendida)
                .Take(limite)
                .ToListAsync();

            // Obtener información adicional de los productos
            var resultado = new List<ProductoMasVendidoDto>();
            foreach (var item in productosMasVendidos)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto != null)
                {
                    resultado.Add(new ProductoMasVendidoDto
                    {
                        ProductoId = item.ProductoId,
                        Nombre = producto.Nombre,
                        CantidadVendida = item.CantidadVendida,
                        TotalVendido = item.TotalVendido,
                        UrlImagen = producto.UrlImagen
                    });
                }
            }

            return resultado;
        }

        public async Task<List<VentaMensualDto>> GetVentasMensualesAsync(int numeroMeses = 12)
        {
            // Fecha de inicio (hace x meses)
            var fechaInicio = DateTime.Today.AddMonths(-numeroMeses + 1).Date;

            // Obtener todos los pedidos desde esa fecha
            var pedidos = await _context.Pedidos
                .Where(p => p.FechaPedido >= fechaInicio)
                .ToListAsync();

            // Agrupar por mes y calcular el total
            var resultado = new List<VentaMensualDto>();
            
            // Crear un diccionario para almacenar los totales por mes
            var ventasPorMes = new Dictionary<string, decimal>();
            
            // Inicializar todos los meses con 0
            for (int i = 0; i < numeroMeses; i++)
            {
                var fecha = DateTime.Today.AddMonths(-i);
                var nombreMes = fecha.ToString("MMM yyyy");
                ventasPorMes[nombreMes] = 0;
            }
            
            // Sumar los totales de los pedidos por mes
            foreach (var pedido in pedidos)
            {
                var nombreMes = pedido.FechaPedido.ToString("MMM yyyy");
                if (ventasPorMes.ContainsKey(nombreMes))
                {
                    ventasPorMes[nombreMes] += pedido.Total;
                }
            }
            
            // Convertir el diccionario a la lista de DTOs (ordenados por fecha)
            foreach (var kvp in ventasPorMes.OrderBy(x => DateTime.ParseExact(x.Key, "MMM yyyy", null)))
            {
                resultado.Add(new VentaMensualDto
                {
                    Mes = kvp.Key,
                    Total = kvp.Value
                });
            }

            return resultado;
        }

        public async Task<List<VentasPorCategoriaDto>> GetVentasPorCategoriaAsync()
        {
            // Necesitamos unir varias tablas para obtener las ventas por categoría
            var ventasPorCategoria = await (
                from detalle in _context.DetallesPedido
                join producto in _context.Productos on detalle.ProductoId equals producto.Id
                join categoria in _context.Categorias on producto.CategoriaId equals categoria.Id
                group new { detalle, categoria } by new { categoria.Id, categoria.Nombre } into g
                select new
                {
                    CategoriaId = g.Key.Id,
                    Nombre = g.Key.Nombre,
                    Total = g.Sum(x => x.detalle.Cantidad * x.detalle.PrecioUnitario)
                }
            ).ToListAsync();

            // Calcular el total general para los porcentajes
            var totalVentas = ventasPorCategoria.Sum(x => x.Total);

            // Convertir a DTO con porcentajes
            var resultado = ventasPorCategoria.Select(x => new VentasPorCategoriaDto
            {
                CategoriaId = x.CategoriaId,
                Nombre = x.Nombre,
                Total = x.Total,
                Porcentaje = totalVentas > 0 ? (double)(x.Total / totalVentas * 100) : 0
            }).ToList();

            return resultado;
        }
    }
}