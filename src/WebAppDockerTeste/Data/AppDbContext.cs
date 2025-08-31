using Microsoft.EntityFrameworkCore;
using WebAppDockerTeste.Models;

namespace WebAppDockerTeste.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var id1 = new Guid("b970630d-91ef-4881-a862-c6a5ef5807d5");
            var id2 = new Guid("0f2e7396-5cef-4541-aaf9-95b92c789122");
            var id3 = new Guid("74226486-7f03-4c66-8473-6453e6f32a8b");

            modelBuilder.Entity<Batman>().HasData(
                new Batman { Id = id1, Name = "Batman1" },
                new Batman { Id = id2, Name = "Batman2" },
                new Batman { Id = id3, Name = "Batman3" }
            );
        }
        public DbSet<Batman> Batmans => Set<Batman>();
    }
}
