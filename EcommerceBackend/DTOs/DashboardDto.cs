using System;
using System.Collections.Generic;

namespace EcommerceBackend.DTOs
{
    public class DashboardDto
    {
        public string Titulo { get; set; } = "Panel de Administración";
        public string Descripcion { get; set; } = "Resumen general del sistema";
        public int TotalUsuarios { get; set; }
        public int TotalProductos { get; set; }
        public int TotalPedidos { get; set; }
        public decimal VentasTotales { get; set; }
        public decimal VentasHoy { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public List<DashboardMetricaDto> Metricas { get; set; } = new List<DashboardMetricaDto>();
        public List<ProductoMasVendidoDto> ProductosMasVendidos { get; set; } = new List<ProductoMasVendidoDto>();
        public List<VentaMensualDto> VentasMensuales { get; set; } = new List<VentaMensualDto>();
        public List<VentasPorCategoriaDto> VentasPorCategoria { get; set; } = new List<VentasPorCategoriaDto>();
    }
}