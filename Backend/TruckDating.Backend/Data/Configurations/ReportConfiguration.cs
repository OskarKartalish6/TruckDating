using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models.Entities;
using TruckDating.Backend.Models.Enums;

namespace TruckDating.Backend.Data.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> entity)
    {
        entity.ToTable("reports", table =>
            table.HasCheckConstraint(
                "ck_reports_different_users",
                "reporter_id <> reported_user_id"));

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        entity.Property(x => x.ReporterId).HasColumnName("reporter_id");
        entity.Property(x => x.ReportedUserId).HasColumnName("reported_user_id");

        entity.Property(x => x.Reason)
            .HasColumnName("reason")
            .HasColumnType("report_reason")
            .HasConversion(
                x => x.ToString().ToLower(),
                x => Enum.Parse<ReportReason>(x, true));

        entity.Property(x => x.Description).HasColumnName("description");

        entity.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("report_status")
            .HasConversion(
                x => x.ToString().ToLower(),
                x => Enum.Parse<ReportStatus>(x, true))
            .HasDefaultValue(ReportStatus.Pending);

        entity.Property(x => x.AssignedAdminId).HasColumnName("assigned_admin_id");
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        entity.Property(x => x.ResolvedAt).HasColumnName("resolved_at");

        entity.HasIndex(x => x.ReportedUserId);
        entity.HasIndex(x => x.Status);

        entity.HasOne(x => x.Reporter)
            .WithMany()
            .HasForeignKey(x => x.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.ReportedUser)
            .WithMany()
            .HasForeignKey(x => x.ReportedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.AssignedAdmin)
            .WithMany()
            .HasForeignKey(x => x.AssignedAdminId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}