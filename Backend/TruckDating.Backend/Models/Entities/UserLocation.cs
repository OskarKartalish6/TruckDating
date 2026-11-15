using NetTopologySuite.Geometries;

namespace TruckDating.Backend.Models.Entities;

public class UserLocation
{
    public Guid UserId { get; set; }

    public Point Location { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
}