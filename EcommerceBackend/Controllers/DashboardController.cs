// Controllers/DashboardController.cs
using System;
using System.Threading.Tasks;
using EcommerceBackend.DTOs;
using EcommerceBackend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Aseguramos que solo los administradores puedan acceder
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboard()
        {
            try
            {
                var dashboard = await _dashboardService.GetDashboardDataAsync();
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                // En producción, deberías loguear la excepción
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}