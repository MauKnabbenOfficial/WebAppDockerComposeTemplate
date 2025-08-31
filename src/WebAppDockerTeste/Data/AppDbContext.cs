using Microsoft.EntityFrameworkCore;
using WebAppDockerTeste.Models;

namespace WebAppDockerTeste.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batman>().HasData(
                new Batman { Id = 1, Name = "Batman1" },
                new Batman { Id = 2, Name = "Batman2" },
                new Batman { Id = 3, Name = "Batman3" }
            );
        }
        public DbSet<Batman> Batmans => Set<Batman>();
    }
}
