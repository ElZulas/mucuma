using Microsoft.AspNetCore.Mvc;

namespace PresupuestoFamiliarMensual.API.Controllers;

/// <summary>
/// Controlador para la página de inicio
/// </summary>
[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    /// <summary>
    /// Página de inicio de la API
    /// </summary>
    /// <returns>Información de la API</returns>
    [HttpGet]
    public ActionResult Get()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        
        return Ok(new
        {
            message = "¡Bienvenido a la API de Presupuesto Familiar Mensual!",
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
            timestamp = DateTime.UtcNow,
            endpoints = new
            {
                health = $"{baseUrl}/api/health",
                healthSimple = $"{baseUrl}/api/health/simple",
                swagger = $"{baseUrl}/swagger",
                budgets = $"{baseUrl}/api/budgets",
                budgetCategories = $"{baseUrl}/api/budgetcategories",
                expenses = $"{baseUrl}/api/expenses",
                referenceData = $"{baseUrl}/api/referencedata"
            },
            documentation = "Para ver la documentación completa de la API, visita /swagger"
        });
    }

    /// <summary>
    /// Endpoint de estado simple
    /// </summary>
    /// <returns>Estado de la API</returns>
    [HttpGet("status")]
    public ActionResult GetStatus()
    {
        return Ok(new
        {
            status = "running",
            service = "Presupuesto Familiar Mensual API",
            timestamp = DateTime.UtcNow
        });
    }
} 