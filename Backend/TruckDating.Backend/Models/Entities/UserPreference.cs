using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Models;

public class UserPreference
{
    public Guid UserId { get; set; }

    public int MinAge { get; set; } = 18;

    public int MaxAge { get; set; } = 99;

    public int? MaxDistanceKm { get; set; } = 100;

    public bool ShowOnlyDrivers { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
}