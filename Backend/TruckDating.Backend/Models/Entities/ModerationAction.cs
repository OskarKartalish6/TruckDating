using TruckDating.Backend.Models.Enums;

namespace TruckDating.Backend.Models.Entities;

public class ModerationAction
{
    public Guid Id { get; set; }

    public Guid AdminId { get; set; }

    public Guid? TargetUserId { get; set; }

    public Guid? ReportId { get; set; }

    public ModerationActionType Action { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public AdminAccount Admin { get; set; } = null!;

    public User? TargetUser { get; set; }

    public Report? Report { get; set; }
}