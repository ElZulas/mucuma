using Microsoft.AspNetCore.Mvc;

namespace PresupuestoFamiliarMensual.API.Controllers;

/// <summary>
/// Controlador para pruebas sin base de datos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    /// <summary>
    /// Prueba de POST sin base de datos
    /// </summary>
    /// <returns>Respuesta de prueba</returns>
    [HttpPost]
    public ActionResult Post([FromBody] object data)
    {
        return Ok(new
        {
            message = "POST funcionando correctamente",
            receivedData = data,
            timestamp = DateTime.UtcNow,
            status = "success"
        });
    }

    /// <summary>
    /// Prueba de GET sin base de datos
    /// </summary>
    /// <returns>Respuesta de prueba</returns>
    [HttpGet]
    public ActionResult Get()
    {
        return Ok(new
        {
            message = "GET funcionando correctamente",
            timestamp = DateTime.UtcNow,
            status = "success"
        });
    }

    /// <summary>
    /// Prueba con parámetros
    /// </summary>
    /// <param name="id">ID de prueba</param>
    /// <returns>Respuesta de prueba</returns>
    [HttpGet("{id}")]
    public ActionResult GetById(int id)
    {
        return Ok(new
        {
            message = $"GET con ID {id} funcionando correctamente",
            id = id,
            timestamp = DateTime.UtcNow,
            status = "success"
        });
    }

    /// <summary>
    /// Prueba de PUT sin base de datos
    /// </summary>
    /// <param name="id">ID de prueba</param>
    /// <param name="data">Datos de prueba</param>
    /// <returns>Respuesta de prueba</returns>
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] object data)
    {
        return Ok(new
        {
            message = $"PUT con ID {id} funcionando correctamente",
            id = id,
            receivedData = data,
            timestamp = DateTime.UtcNow,
            status = "success"
        });
    }

    /// <summary>
    /// Prueba de DELETE sin base de datos
    /// </summary>
    /// <param name="id">ID de prueba</param>
    /// <returns>Respuesta de prueba</returns>
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        return Ok(new
        {
            message = $"DELETE con ID {id} funcionando correctamente",
            id = id,
            timestamp = DateTime.UtcNow,
            status = "success"
        });
    }
} 