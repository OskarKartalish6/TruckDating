using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruckDating.Backend.Models.Entities;

namespace TruckDating.Backend.Data.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> entity)
    {
        entity.ToTable("messages", table =>
            table.HasCheckConstraint(
                "ck_messages_content",
                "length(trim(content)) > 0"));

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        entity.Property(x => x.ChatId).HasColumnName("chat_id");
        entity.Property(x => x.SenderId).HasColumnName("sender_id");
        entity.Property(x => x.Content).HasColumnName("content").IsRequired();
        entity.Property(x => x.IsRead).HasColumnName("is_read").HasDefaultValue(false);
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        entity.HasIndex(x => new { x.ChatId, x.CreatedAt });

        entity.HasOne(x => x.Chat)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        // Проверяем, что отправитель является участником именно этого чата.
        entity.HasOne<ChatParticipant>()
            .WithMany()
            .HasForeignKey(x => new { x.ChatId, x.SenderId })
            .HasPrincipalKey(x => new { x.ChatId, x.UserId })
            .OnDelete(DeleteBehavior.Restrict);

        // Sender navigation не является отдельным FK в существующей БД.
        entity.Ignore(x => x.Sender);
    }
}