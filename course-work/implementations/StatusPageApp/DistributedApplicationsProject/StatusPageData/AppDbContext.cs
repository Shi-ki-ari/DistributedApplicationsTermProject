using Microsoft.EntityFrameworkCore;
using StatusPageData.Entities;

namespace StatusPageData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<IncidentEntity> IncidentsEntities { get; set; } = null!;
        public DbSet<EngineerEntity> EngineersEntities { get; set; } = null!;
        public DbSet<ServiceEntity> Services { get; set; } = null!;
        public DbSet<ServiceCategoryEntity> ServiceCategories { get; set; } = null!;
        public DbSet<ServiceCheckEntity> ServiceChecks { get; set; } = null!;
        public DbSet<IncidentUpdateEntity> IncidentsUpdates { get; set; } = null!;

        public DbSet<UserEntity> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<IncidentEntity>().
                HasOne(i => i.AssignedEngineer)
                .WithMany(e => e.AssignedIncidents)
                .HasForeignKey(i => i.AssignedEngineerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IncidentUpdateEntity>()
                .HasOne(u => u.Engineer)
                .WithMany(e => e.IncidentUpdates)
                .HasForeignKey(u => u.EngineerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IncidentUpdateEntity>()
                .HasOne(i => i.Incident)
                .WithMany(u => u.Updates)
                .HasForeignKey(i => i.IncidentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserEntity>()
                .ToTable("Users");


            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    Username = "admin",
                    Password = "password123"
                });

            // Seed some default service categories and services for testing/demo
            modelBuilder.Entity<ServiceCategoryEntity>().HasData(
                new ServiceCategoryEntity
                {
                    Id = 1,
                    Name = "External",
                    Description = "External third-party services",
                    DisplayOrder = 1,
                    CreatedAt = new DateTime(2026, 1, 1),
                    Notify = false
                },
                new ServiceCategoryEntity
                {
                    Id = 2,
                    Name = "Testing",
                    Description = "Test endpoints used for health checks",
                    DisplayOrder = 2,
                    CreatedAt = new DateTime(2026, 1, 2),
                    Notify = false
                }
            );

            modelBuilder.Entity<ServiceEntity>().HasData(
                new ServiceEntity
                {
                    Id = 1,
                    Name = "Google",
                    TargetUrl = "https://www.google.com",
                    IsOnline = true,
                    DateAdded = new DateTime(2026, 1, 1),
                    UptimePercentage = 99.99m,
                    CategoryId = 1
                },
                new ServiceEntity
                {
                    Id = 2,
                    Name = "HttpStat 404",
                    TargetUrl = "https://httpstat.us/404",
                    IsOnline = false,
                    DateAdded = new DateTime(2026, 1, 2),
                    UptimePercentage = 0m,
                    CategoryId = 2
                }
            );

        }


    }
}
