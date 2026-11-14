using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Models;

public class UserProfile
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

    public string? Bio { get; set; }

    public string? Gender { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
}