using PracticaAPI.DTOs;

namespace PracticaAPI.Core.Services.Interfaces;
 
public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto);
} 