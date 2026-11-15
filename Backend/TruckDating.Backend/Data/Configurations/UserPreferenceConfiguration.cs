using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models;
using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Data.Configurations;

public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreference> entity)
    {
        entity.ToTable("user_preferences", table =>
        {
            table.HasCheckConstraint(
                "ck_user_preferences_age",
                "min_age >= 18 AND max_age >= min_age AND max_age <= 120");

            table.HasCheckConstraint(
                "ck_user_preferences_distance",
                "max_distance_km IS NULL OR max_distance_km > 0");
        });

        entity.HasKey(x => x.UserId);

        entity.Property(x => x.UserId).HasColumnName("user_id");
        entity.Property(x => x.MinAge).HasColumnName("min_age").HasDefaultValue(18);
        entity.Property(x => x.MaxAge).HasColumnName("max_age").HasDefaultValue(99);
        entity.Property(x => x.MaxDistanceKm).HasColumnName("max_distance_km").HasDefaultValue(100);
        entity.Property(x => x.ShowOnlyDrivers).HasColumnName("show_only_drivers").HasDefaultValue(false);
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");

        entity.HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<UserPreference>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}