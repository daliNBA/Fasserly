using Fasserly.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fasserly.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserFasserly> UserFasserlies { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
    }
}
