using Microsoft.AspNetCore.Mvc;

namespace PresupuestoFamiliarMensual.API.Controllers;

/// <summary>
/// Controlador para la ruta raíz
/// </summary>
[ApiController]
[Route("")]
public class RootController : ControllerBase
{
    /// <summary>
    /// Página de inicio en la ruta raíz
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
            status = "running",
            endpoints = new
            {
                home = $"{baseUrl}/home",
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
    /// Health check en la ruta raíz
    /// </summary>
    /// <returns>Estado de la API</returns>
    [HttpGet("health")]
    public ActionResult GetHealth()
    {
        return Ok("OK");
    }
} 