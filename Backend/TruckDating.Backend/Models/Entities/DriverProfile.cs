using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Models;

public class DriverProfile
{
    public Guid UserId { get; set; }

    public string? TruckDescription { get; set; }

    public int? ExperienceYears { get; set; }

    public string? PreferredRoutes { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}