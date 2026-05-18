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


            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    Username = "admin",
                    Password = "password123"
                });

        }


    }
}
