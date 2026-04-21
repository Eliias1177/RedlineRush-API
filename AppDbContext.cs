using Microsoft.EntityFrameworkCore;
using DragRacingAPI.Models;

namespace DragRacingAPI
{
    public class AppDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Car> CatalogCars { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}