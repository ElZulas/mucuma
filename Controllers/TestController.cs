using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PracticaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous] // Permitir acceso sin autenticación para pruebas
public class TestController : ControllerBase
{
    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        var clientIp = GetClientIpAddress();
        var allHeaders = GetDetailedIpInfo();
        
        return Ok(new
        {
            Status = "API is running",
            ClientIP = clientIp,
            IsAllowed = clientIp == "187.155.101.200",
            AllowedIP = "187.155.101.200",
            IPDetectionDetails = allHeaders,
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("ip")]
    public IActionResult GetClientIp()
    {
        var clientIp = GetClientIpAddress();
        var allHeaders = GetDetailedIpInfo();
        
        return Ok(new
        {
            ClientIP = clientIp,
            IsAllowed = clientIp == "187.155.101.200",
            AllowedIP = "187.155.101.200",
            Message = clientIp == "187.155.101.200" 
                ? "Your IP is authorized" 
                : "Your IP is not authorized",
            IPDetectionDetails = allHeaders,
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("ip-test")]
    public IActionResult TestIpFilter()
    {
        var clientIp = GetClientIpAddress();
        var allHeaders = GetDetailedIpInfo();
        
        return Ok(new
        {
            TestType = "IP Filter Test",
            ClientIP = clientIp,
            IsAllowed = clientIp == "187.155.101.200",
            AllowedIP = "187.155.101.200",
            Status = clientIp == "187.155.101.200" ? "AUTHORIZED" : "BLOCKED",
            IPDetectionDetails = allHeaders,
            Note = "This endpoint tests the IP filter. Only 187.155.101.200 is allowed.",
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("debug-ip")]
    public IActionResult DebugIpDetection()
    {
        var clientIp = GetClientIpAddress();
        var allowedIp = Environment.GetEnvironmentVariable("AllowedIP");
        var allHeaders = GetDetailedIpInfo();
        return Ok(new
        {
            DebugType = "IP Detection Debug",
            DetectedIP = clientIp,
            ExpectedIP = allowedIp,
            IsMatch = clientIp == allowedIp,
            AllHeaders = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
            ConnectionInfo = new
            {
                RemoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                LocalIpAddress = HttpContext.Connection.LocalIpAddress?.ToString(),
                RemotePort = HttpContext.Connection.RemotePort,
                LocalPort = HttpContext.Connection.LocalPort
            },
            IPDetectionDetails = allHeaders,
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("auth-test")]
    [Authorize] // Este endpoint requiere autenticación
    public IActionResult TestAuth()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        
        return Ok(new
        {
            Message = "Authentication successful!",
            UserId = userId,
            Username = username,
            Timestamp = DateTime.UtcNow
        });
    }

    private string GetClientIpAddress()
    {
        // Obtener la IP real del cliente, considerando proxies y load balancers
        var forwardedHeader = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedHeader))
        {
            // Tomar la primera IP de la lista (IP original del cliente)
            var ips = forwardedHeader.Split(',');
            return ips[0].Trim();
        }

        var realIpHeader = Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIpHeader))
        {
            return realIpHeader.Trim();
        }

        var xForwardedProtoHeader = Request.Headers["X-Forwarded-Proto"].FirstOrDefault();
        if (!string.IsNullOrEmpty(xForwardedProtoHeader))
        {
            // Si hay X-Forwarded-Proto, también verificar X-Forwarded-For
            var forwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }
        }

        // Obtener la IP directa de la conexión
        var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        
        // Si es IPv6, convertir a IPv4 si es posible
        if (remoteIp != null && remoteIp.Contains("::ffff:"))
        {
            remoteIp = remoteIp.Replace("::ffff:", "");
        }

        return remoteIp ?? "Unknown";
    }

    private object GetDetailedIpInfo()
    {
        return new
        {
            XForwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault(),
            XRealIP = Request.Headers["X-Real-IP"].FirstOrDefault(),
            XForwardedProto = Request.Headers["X-Forwarded-Proto"].FirstOrDefault(),
            XForwardedHost = Request.Headers["X-Forwarded-Host"].FirstOrDefault(),
            RemoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            LocalIpAddress = HttpContext.Connection.LocalIpAddress?.ToString(),
            UserAgent = Request.Headers["User-Agent"].FirstOrDefault()
        };
    }
} 