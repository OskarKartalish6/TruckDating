using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Data.Configurations;

public class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> entity)
    {
        entity.ToTable("chat_participants");

        entity.HasKey(x => new { x.ChatId, x.UserId });

        entity.Property(x => x.ChatId).HasColumnName("chat_id");
        entity.Property(x => x.UserId).HasColumnName("user_id");
        entity.Property(x => x.JoinedAt).HasColumnName("joined_at").HasDefaultValueSql("now()");

        entity.HasOne(x => x.Chat)
            .WithMany(x => x.Participants)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}