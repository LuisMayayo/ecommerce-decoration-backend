using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.DTOs;

namespace EcommerceBackend.Interfaces
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalUsuariosAsync();
        Task<int> GetTotalProductosAsync();
        Task<int> GetTotalPedidosAsync();
        Task<decimal> GetVentasTotalesAsync();
        Task<decimal> GetVentasHoyAsync();
        Task<List<ProductoMasVendidoDto>> GetProductosMasVendidosAsync(int limite = 5);
        Task<List<VentaMensualDto>> GetVentasMensualesAsync(int numeroMeses = 12);
        Task<List<VentasPorCategoriaDto>> GetVentasPorCategoriaAsync();
    }
}
