namespace TruckDating.Backend.Models.Entities;

public class Block
{
    public Guid Id { get; set; }

    public Guid BlockerId { get; set; }

    public Guid BlockedId { get; set; }

    public DateTime CreatedAt { get; set; }

    public User Blocker { get; set; } = null!;

    public User Blocked { get; set; } = null!;
}