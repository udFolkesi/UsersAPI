using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Contexts
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
        {
            //Database.EnsureDeleted(); // recreate bd
            //Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
    }
}
