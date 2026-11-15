using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Data.Configurations;

public class BlockConfiguration : IEntityTypeConfiguration<Block>
{
    public void Configure(EntityTypeBuilder<Block> entity)
    {
        entity.ToTable("blocks", table =>
            table.HasCheckConstraint(
                "ck_blocks_different_users",
                "blocker_id <> blocked_id"));

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        entity.Property(x => x.BlockerId).HasColumnName("blocker_id");
        entity.Property(x => x.BlockedId).HasColumnName("blocked_id");
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        entity.HasIndex(x => new { x.BlockerId, x.BlockedId }).IsUnique();

        entity.HasOne(x => x.Blocker)
            .WithMany()
            .HasForeignKey(x => x.BlockerId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.Blocked)
            .WithMany()
            .HasForeignKey(x => x.BlockedId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}