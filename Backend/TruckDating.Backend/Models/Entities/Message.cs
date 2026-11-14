namespace TruckDating.Backend.Models.Entities;

public class Message
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public Guid SenderId { get; set; }

    public string Content { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public Chat Chat { get; set; } = null!;

    public User Sender { get; set; } = null!;
}