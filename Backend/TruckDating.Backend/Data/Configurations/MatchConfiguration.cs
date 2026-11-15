using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Data.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> entity)
    {
        entity.ToTable("matches", table =>
            table.HasCheckConstraint(
                "ck_matches_user_order",
                "user_one_id < user_two_id"));

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        entity.Property(x => x.UserOneId).HasColumnName("user_one_id");
        entity.Property(x => x.UserTwoId).HasColumnName("user_two_id");
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        entity.HasIndex(x => new { x.UserOneId, x.UserTwoId }).IsUnique();
        entity.HasIndex(x => x.UserTwoId);

        entity.HasOne(x => x.UserOne)
            .WithMany()
            .HasForeignKey(x => x.UserOneId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.UserTwo)
            .WithMany()
            .HasForeignKey(x => x.UserTwoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}