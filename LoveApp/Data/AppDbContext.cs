using Microsoft.EntityFrameworkCore;
using LoveApp.Models;
using System;

namespace LoveApp.Data
{
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

        // Daily Questions
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cascade delete for MemoryPin -> Images
            modelBuilder.Entity<MemoryPin>()
                .HasMany(p => p.Images)
                .WithOne(i => i.MemoryPin)
                .HasForeignKey(i => i.MemoryPinId)
                .OnDelete(DeleteBehavior.Cascade);

            // Helper: convert DateTime properties to UTC
            void ConvertToUtc<T>(string propertyName) where T : class
            {
                modelBuilder.Entity<T>()
                    .Property<DateTime>(propertyName)
                    .HasConversion(
                        v => v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
            }

            // Apply UTC conversion for all relevant fields
            ConvertToUtc<Note>("CreatedAt");
            ConvertToUtc<Notification>("CreatedAt");
            ConvertToUtc<Visit>("CreatedAt");
            ConvertToUtc<Visit>("StartDate");
            ConvertToUtc<Visit>("EndDate");

            ConvertToUtc<DailyWords>("CreatedAt");

            ConvertToUtc<MysteryMessage>("CreatedAt");
            ConvertToUtc<MysteryVisit>("LastOpened");
            ConvertToUtc<SentMysteryMessages>("SentAt");

            ConvertToUtc<Drawing>("CreatedAt");
            ConvertToUtc<Drawing>("UpdatedAt");

            ConvertToUtc<Answer>("AnsweredAt"); // new: store answers in UTC
        }
    }
}
