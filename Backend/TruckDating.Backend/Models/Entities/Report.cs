using TruckDating.Backend.Models.Enums;

namespace TruckDating.Backend.Models.Entities;

public class Report
{
    public Guid Id { get; set; }

    public Guid ReporterId { get; set; }

    public Guid ReportedUserId { get; set; }

    public ReportReason Reason { get; set; }

    public string? Description { get; set; }

    public ReportStatus Status { get; set; } = ReportStatus.Pending;

    public Guid? AssignedAdminId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public User Reporter { get; set; } = null!;

    public User ReportedUser { get; set; } = null!;

    public AdminAccount? AssignedAdmin { get; set; }
}