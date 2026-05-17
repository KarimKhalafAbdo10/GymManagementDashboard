using GemMangement.Configurations;
using GemMangement.Models;
using Microsoft.EntityFrameworkCore;

namespace GemMangement.DbContexts
{
    public class GymDbContext:DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=GemMangement;Trusted_Connection=true; TrustServerCertificate=true");
        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlanConfiguration());
        }
        public DbSet<Plan> Plans { get; set; }
    }
}
