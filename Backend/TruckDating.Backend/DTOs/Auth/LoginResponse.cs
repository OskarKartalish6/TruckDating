namespace TruckDating.Backend.DTOs.Auth;

public class LoginResponse
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}