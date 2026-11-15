using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models.Entities;
using TruckDating.Backend.Models.Enums;

namespace TruckDating.Backend.Data.Configurations;

public class ModerationActionConfiguration : IEntityTypeConfiguration<ModerationAction>
{
    public void Configure(EntityTypeBuilder<ModerationAction> entity)
    {
        entity.ToTable("moderation_actions");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        entity.Property(x => x.AdminId).HasColumnName("admin_id");
        entity.Property(x => x.TargetUserId).HasColumnName("target_user_id");
        entity.Property(x => x.ReportId).HasColumnName("report_id");

        entity.Property(x => x.Action)
            .HasColumnName("action")
            .HasColumnType("moderation_action_type");

        entity.Property(x => x.Description).HasColumnName("description");
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        entity.HasOne(x => x.Admin)
            .WithMany()
            .HasForeignKey(x => x.AdminId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.TargetUser)
            .WithMany()
            .HasForeignKey(x => x.TargetUserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Report)
            .WithMany()
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}