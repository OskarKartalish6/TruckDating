using Microsoft.EntityFrameworkCore;
using TruckDating.Backend.Models;
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
    public DbSet<AdminAccount> AdminAccounts { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<DriverProfile> DriverProfiles { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<UserPhoto> UserPhotos { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserLocation> UserLocations { get; set; }
    public DbSet<RouteStop> RouteStops { get; set; }
    public DbSet<Block> Blocks { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<ModerationAction> ModerationActions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PostgreSQL ENUM types
        modelBuilder.HasPostgresEnum<AccountStatus>();
        modelBuilder.HasPostgresEnum<ModerationActionType>();
        modelBuilder.HasPostgresEnum<PhotoStatus>();
        modelBuilder.HasPostgresEnum<ReportReason>();
        modelBuilder.HasPostgresEnum<ReportStatus>();

        // Entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly
        );
    }

}