using Microsoft.EntityFrameworkCore;
using LoveApp.Models;

namespace LoveApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Note> Notes { get; set; } = null!;
    public DbSet<Visit> Visits { get; set; } = null!;
    public DbSet<MysteryMessage> MysteryMessages { get; set; } = null!;
    public DbSet<MysteryVisit> MysteryVisits { get; set; } = null!;
    public DbSet<SentMysteryMessages> SentMysteryMessages { get; set; } = null!;
    public DbSet<MemoryPin> MemoryPins { get; set; } = null!;
    public DbSet<MemoryImage> MemoryImages { get; set; } = null!;
    public DbSet<Drawing> Drawings { get; set; } = null!;
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<TripPoint> TripPoints => Set<TripPoint>();
    public DbSet<DailyWords> DailyWords => Set<DailyWords>();


    // Convert all DateTime properties to UTC automatically
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cascade delete setup
        modelBuilder.Entity<MemoryPin>()
            .HasMany(p => p.Images)
            .WithOne(i => i.MemoryPin)
            .HasForeignKey(i => i.MemoryPinId)
            .OnDelete(DeleteBehavior.Cascade);

        // Convert specific DateTime fields to UTC
        modelBuilder.Entity<Note>()
            .Property(n => n.CreatedAt)
            .HasConversion(
                v => v.ToUniversalTime(),                        // store as UTC
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)   // read as UTC
            );

        modelBuilder.Entity<Visit>()
            .Property(n => n.CreatedAt)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
        modelBuilder.Entity<Visit>()
            .Property(n => n.EndDate)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
        modelBuilder.Entity<Visit>()
            .Property(n => n.StartDate)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
        modelBuilder.Entity<DailyWords>()
            .Property(n => n.CreatedAt)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
        modelBuilder.Entity<MysteryMessage>()
            .Property(n => n.CreatedAt)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
        modelBuilder.Entity<MysteryVisit>()
            .Property(n => n.LastOpened)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
        modelBuilder.Entity<SentMysteryMessages>()
            .Property(n => n.SentAt)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
        modelBuilder.Entity<Drawing>()
            .Property(n => n.UpdatedAt)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
        modelBuilder.Entity<Drawing>()
    .Property(n => n.CreatedAt)
    .HasConversion(
        v => v.ToUniversalTime(),
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
    );
    }

}
