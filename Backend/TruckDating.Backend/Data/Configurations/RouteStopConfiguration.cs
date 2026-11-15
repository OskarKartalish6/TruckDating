using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Data.Configurations;

public class RouteStopConfiguration : IEntityTypeConfiguration<RouteStop>
{
    public void Configure(EntityTypeBuilder<RouteStop> entity)
    {
        entity.ToTable("route_stops");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        entity.Property(x => x.UserId).HasColumnName("user_id");
        entity.Property(x => x.Name).HasColumnName("name").IsRequired();

        entity.Property(x => x.Location)
            .HasColumnName("location")
            .HasColumnType("geography(Point,4326)");

        entity.Property(x => x.PlannedArrival).HasColumnName("planned_arrival");
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        entity.HasIndex(x => x.UserId);

        entity.HasIndex(x => x.Location)
            .HasMethod("gist");

        entity.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}