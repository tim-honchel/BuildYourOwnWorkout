using Microsoft.EntityFrameworkCore;
using Workouts.Entities.Database;

namespace Workouts.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Workout> Routine { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
