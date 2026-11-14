using TruckDating.Backend.Models.Entities;
using TruckDating.Backend.Models.Enums;

namespace TruckDating.Backend.Models;

public class UserPhoto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string StorageKey { get; set; } = string.Empty;

    public PhotoStatus Status { get; set; } = PhotoStatus.Pending;

    public bool IsPrimary { get; set; }

    public Guid? ReviewedByAdminId { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;

    public AdminAccount? ReviewedByAdmin { get; set; }
}