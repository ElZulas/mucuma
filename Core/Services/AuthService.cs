using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PracticaAPI.Core.Entities;
using PracticaAPI.Core.Services.Interfaces;
using PracticaAPI.Data;
using PracticaAPI.DTOs;

namespace PracticaAPI.Core.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);

        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Usuario o contraseña incorrectos"
            };
        }

        // Actualizar último login
        user.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Success = true,
            Token = token,
            Username = user.Username,
            Message = "Login exitoso"
        };
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        // Validar que las contraseñas coincidan
        if (registerDto.Password != registerDto.ConfirmPassword)
        {
            return new RegisterResponseDto
            {
                Success = false,
                Message = "Las contraseñas no coinciden"
            };
        }

        // Verificar si el usuario ya existe
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == registerDto.Username);

        if (existingUser != null)
        {
            return new RegisterResponseDto
            {
                Success = false,
                Message = "El usuario ya existe"
            };
        }

        // Crear nuevo usuario
        var user = new User
        {
            Username = registerDto.Username,
            PasswordHash = HashPassword(registerDto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new RegisterResponseDto
        {
            Success = true,
            Username = user.Username,
            Message = "Usuario creado exitosamente"
        };
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKey12345678901234567890"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "PracticaAPI",
            audience: _configuration["Jwt:Audience"] ?? "PracticaAPI",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
} 