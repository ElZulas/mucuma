using Microsoft.AspNetCore.Mvc;

namespace PresupuestoFamiliarMensual.API.Controllers;

/// <summary>
/// Controlador para health checks
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Health check básico para Railway
    /// </summary>
    /// <returns>Estado de la aplicación</returns>
    [HttpGet]
    public ActionResult Get()
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
        var hasConnectionString = !string.IsNullOrEmpty(connectionString);
        
        return Ok(new { 
            status = "healthy", 
            timestamp = DateTime.UtcNow,
            service = "Presupuesto Familiar Mensual API",
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
            database = new {
                hasConnectionString = hasConnectionString,
                connectionStringLength = connectionString?.Length ?? 0,
                dbHost = Environment.GetEnvironmentVariable("DB_HOST"),
                dbPort = Environment.GetEnvironmentVariable("DB_PORT"),
                dbName = Environment.GetEnvironmentVariable("DB_NAME"),
                dbUser = Environment.GetEnvironmentVariable("DB_USER"),
                hasDbPassword = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_PASSWORD"))
            }
        });
    }

    /// <summary>
    /// Health check simple para Railway
    /// </summary>
    /// <returns>Estado de la aplicación</returns>
    [HttpGet("simple")]
    public ActionResult GetSimple()
    {
        return Ok("OK");
    }
} 