using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models;
using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Data.Configurations;

public class DriverProfileConfiguration : IEntityTypeConfiguration<DriverProfile>
{
    public void Configure(EntityTypeBuilder<DriverProfile> entity)
    {
        entity.ToTable("driver_profiles", table =>
            table.HasCheckConstraint(
                "ck_driver_profiles_experience",
                "experience_years IS NULL OR experience_years >= 0"));

        entity.HasKey(x => x.UserId);

        entity.Property(x => x.UserId).HasColumnName("user_id");
        entity.Property(x => x.TruckDescription).HasColumnName("truck_description");
        entity.Property(x => x.ExperienceYears).HasColumnName("experience_years");
        entity.Property(x => x.PreferredRoutes).HasColumnName("preferred_routes");
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        entity.HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<DriverProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}