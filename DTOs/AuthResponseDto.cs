namespace PracticaAPI.DTOs;

public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class RegisterResponseDto
{
    public bool Success { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
} 