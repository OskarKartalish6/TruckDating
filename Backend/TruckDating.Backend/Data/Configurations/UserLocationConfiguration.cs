using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Data.Configurations;

public class UserLocationConfiguration : IEntityTypeConfiguration<UserLocation>
{
    public void Configure(EntityTypeBuilder<UserLocation> entity)
    {
        entity.ToTable("user_locations");

        entity.HasKey(x => x.UserId);

        entity.Property(x => x.UserId).HasColumnName("user_id");

        entity.Property(x => x.Location)
            .HasColumnName("location")
            .HasColumnType("geography(Point,4326)");

        entity.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("now()");

        entity.HasIndex(x => x.Location)
            .HasMethod("gist");

        entity.HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<UserLocation>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}