using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StatusPageData.Entities;

namespace StatusPageData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<IncidentsEntity> IncidentsEntities { get; set; } = null!;
        public DbSet<EngineersEntity> EngineersEntities { get; set; } = null!;
        public DbSet<Services> Services { get; set; } = null!;
        public DbSet<ServiceCategories> ServiceCategories { get; set; } = null!;
        public DbSet<IncidentsUpdates> IncidentsUpdates { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<IncidentsEntity>().
                HasOne(i => i.AssignedEngineer)
                .WithMany(e => e.AssignedIncidents)
                .HasForeignKey(i => i.AssignedEngineerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IncidentsUpdates>()
                .HasOne(u => u.Engineer)
                .WithMany(e => e.IncidentUpdates)
                .HasForeignKey(u => u.EngineerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IncidentsUpdates>()
                .HasOne(i => i.Incident)
                .WithMany(u => u.Updates)
                .HasForeignKey(i => i.IncidentId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
