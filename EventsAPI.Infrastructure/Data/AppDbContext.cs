using Microsoft.EntityFrameworkCore;
using EventsAPI.Core.Entities;

namespace EventsAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { Database.EnsureCreated(); }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=eventsapi.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EventEntityModeling(modelBuilder);
        }

        private static void EventEntityModeling(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(e => e.Type)
                .IsRequired();
            modelBuilder.Entity<Event>()
                .Property(e => e.AddedOn)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Event>()
                .Property(e => e.LastModifiedOn)
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}