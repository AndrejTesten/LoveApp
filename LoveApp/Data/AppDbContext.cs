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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MemoryPin>()
            .HasMany(p => p.Images)
            .WithOne(i => i.MemoryPin)
            .HasForeignKey(i => i.MemoryPinId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
