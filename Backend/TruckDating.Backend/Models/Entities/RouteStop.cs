using NetTopologySuite.Geometries;

namespace TruckDating.Backend.Models.Entities;

public class RouteStop
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public Point Location { get; set; } = null!;

    public DateTime? PlannedArrival { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}