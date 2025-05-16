// Services/DashboardService.cs
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EcommerceBackend.DTOs;
using EcommerceBackend.Interfaces;

namespace EcommerceBackend.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            // Crear el objeto DTO del dashboard
            var dashboardDto = new DashboardDto
            {
                Titulo = "Panel de Administración",
                Descripcion = "Resumen del E-commerce",
                UltimaActualizacion = DateTime.Now
            };

            // Obtener datos básicos
            dashboardDto.TotalUsuarios = await _dashboardRepository.GetTotalUsuariosAsync();
            dashboardDto.TotalProductos = await _dashboardRepository.GetTotalProductosAsync();
            dashboardDto.TotalPedidos = await _dashboardRepository.GetTotalPedidosAsync();
            dashboardDto.VentasTotales = await _dashboardRepository.GetVentasTotalesAsync();
            dashboardDto.VentasHoy = await _dashboardRepository.GetVentasHoyAsync();

            // Obtener datos para gráficos y estadísticas
            dashboardDto.ProductosMasVendidos = await _dashboardRepository.GetProductosMasVendidosAsync();
            dashboardDto.VentasMensuales = await _dashboardRepository.GetVentasMensualesAsync();
            dashboardDto.VentasPorCategoria = await _dashboardRepository.GetVentasPorCategoriaAsync();

            // Crear métricas para el dashboard
            dashboardDto.Metricas = new List<DashboardMetricaDto>
            {
                new DashboardMetricaDto
                {
                    Nombre = "Usuarios",
                    Valor = dashboardDto.TotalUsuarios.ToString(),
                    Icono = "fa-users",
                    ColorClase = "bg-primary"
                },
                new DashboardMetricaDto
                {
                    Nombre = "Productos",
                    Valor = dashboardDto.TotalProductos.ToString(),
                    Icono = "fa-box",
                    ColorClase = "bg-success"
                },
                new DashboardMetricaDto
                {
                    Nombre = "Pedidos",
                    Valor = dashboardDto.TotalPedidos.ToString(),
                    Icono = "fa-shopping-cart",
                    ColorClase = "bg-info"
                },
                new DashboardMetricaDto
                {
                    Nombre = "Ventas Totales",
                    Valor = $"{dashboardDto.VentasTotales:C}",
                    Icono = "fa-euro-sign",
                    ColorClase = "bg-warning"
                },
                new DashboardMetricaDto
                {
                    Nombre = "Ventas Hoy",
                    Valor = $"{dashboardDto.VentasHoy:C}",
                    Icono = "fa-calendar-day",
                    ColorClase = "bg-danger"
                }
            };

            return dashboardDto;
        }
    }
}