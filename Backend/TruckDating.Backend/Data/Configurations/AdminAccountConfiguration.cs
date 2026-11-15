
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Data.Configurations;

public class AdminAccountConfiguration
    : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> entity)
    {
        entity.ToTable("admin_accounts");

        entity.HasKey(a => a.Id);

        entity.Property(a => a.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        entity.Property(a => a.Email)
            .HasColumnName("email")
            .IsRequired();

        entity.HasIndex(a => a.Email)
            .IsUnique();

        entity.Property(a => a.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();

        entity.Property(a => a.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        entity.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()");

        entity.Property(a => a.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("now()");
    }
}
