using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models;
using TruckDating.Backend.Models.Entities;
using TruckDating.Backend.Models.Enums;

namespace TruckDating.Backend.Data.Configurations;

public class UserPhotoConfiguration : IEntityTypeConfiguration<UserPhoto>
{
    public void Configure(EntityTypeBuilder<UserPhoto> entity)
    {
        entity.ToTable("user_photos");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        entity.Property(x => x.UserId).HasColumnName("user_id");
        entity.Property(x => x.StorageKey).HasColumnName("storage_key").IsRequired();

        entity.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("photo_status")
            .HasConversion(
                x => x.ToString().ToLower(),
                x => Enum.Parse<PhotoStatus>(x, true))
            .HasDefaultValue(PhotoStatus.Pending);

        entity.Property(x => x.IsPrimary).HasColumnName("is_primary").HasDefaultValue(false);
        entity.Property(x => x.ReviewedByAdminId).HasColumnName("reviewed_by_admin_id");
        entity.Property(x => x.ReviewedAt).HasColumnName("reviewed_at");
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        entity.HasIndex(x => x.UserId);

        entity.HasIndex(x => x.UserId)
            .IsUnique()
            .HasFilter("is_primary = true");

        entity.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.ReviewedByAdmin)
            .WithMany()
            .HasForeignKey(x => x.ReviewedByAdminId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}