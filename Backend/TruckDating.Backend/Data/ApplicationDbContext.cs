using Microsoft.EntityFrameworkCore;
using TruckDating.Backend.Models.Entities;
using TruckDating.Backend.Models.Enums;

namespace TruckDating.Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired();

            entity.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            entity.Property(u => u.Status)
                .HasColumnName("status")
                .HasColumnType("account_status")
                .HasConversion(
                    status => status.ToString().ToLower(),
                    value => Enum.Parse<AccountStatus>(
                        value, true
                    )
                )
                .HasDefaultValue(AccountStatus.Active);

            entity.Property(u => u.EmailVerified)
                .HasColumnName("email_verified")
                .HasDefaultValue(false);

            entity.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");

            entity.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("now()");
        });
    }
}