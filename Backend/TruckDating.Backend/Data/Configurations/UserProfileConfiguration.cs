using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models;
using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> entity)
    {
        entity.ToTable("user_profiles");

        entity.HasKey(x => x.UserId);

        entity.Property(x => x.UserId).HasColumnName("user_id");
        entity.Property(x => x.FirstName).HasColumnName("first_name").IsRequired();
        entity.Property(x => x.DateOfBirth).HasColumnName("date_of_birth").IsRequired();
        entity.Property(x => x.Bio).HasColumnName("bio");
        entity.Property(x => x.Gender).HasColumnName("gender");
        entity.Property(x => x.Country).HasColumnName("country");
        entity.Property(x => x.City).HasColumnName("city");
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");

        entity.HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<UserProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}