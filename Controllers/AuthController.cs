using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PracticaAPI.Core.Services.Interfaces;
using PracticaAPI.DTOs;

namespace PracticaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(loginDto);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(registerDto);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    // GET: api/auth/me (para obtener información del usuario actual)
    [HttpGet("me")]
    [Authorize] // Requiere autenticación
    public ActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        if (userId == null)
        {
            return Unauthorized("Token inválido");
        }

        return Ok(new
        {
            UserId = userId,
            Username = username
        });
    }
} 