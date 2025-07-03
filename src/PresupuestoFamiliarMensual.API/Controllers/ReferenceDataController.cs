using Microsoft.AspNetCore.Mvc;
using PresupuestoFamiliarMensual.Core.Entities;
using PresupuestoFamiliarMensual.Core.Interfaces;

namespace PresupuestoFamiliarMensual.API.Controllers;

/// <summary>
/// Controlador para obtener datos de referencia
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReferenceDataController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ReferenceDataController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Obtiene todos los miembros de la familia
    /// </summary>
    /// <returns>Lista de miembros de la familia</returns>
    [HttpGet("family-members")]
    public async Task<ActionResult<IEnumerable<FamilyMember>>> GetFamilyMembers()
    {
        try
        {
            var familyMembers = await _unitOfWork.FamilyMembers.GetAllAsync();
            return Ok(familyMembers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene todos los meses disponibles
    /// </summary>
    /// <returns>Lista de meses</returns>
    [HttpGet("months")]
    public async Task<ActionResult<IEnumerable<Month>>> GetMonths()
    {
        try
        {
            var months = await _unitOfWork.Months.GetAllAsync();
            return Ok(months);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Health check endpoint para Railway
    /// </summary>
    /// <returns>Estado de la aplicación</returns>
    [HttpGet("health")]
    public ActionResult GetHealth()
    {
        try
        {
            return Ok(new { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                service = "Presupuesto Familiar Mensual API",
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                status = "unhealthy", 
                timestamp = DateTime.UtcNow,
                error = ex.Message
            });
        }
    }
} 