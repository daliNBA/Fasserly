using Fasserly.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fasserly.Database
{
    public class DatabaseContext : IdentityDbContext<UserFasserly>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<UserTraining> UserTrainings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTraining>(x => x.HasKey(ua => new { ua.UserFasserlyId, ua.TrainingId }));

            //modelBuilder.Entity<Training>()
            //    .HasOne(u => u.Owner)
            //    .WithMany(a => a.Trainings).OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<UserTraining>()
            //    .HasOne(u => u.UserFasserly)
            //    .WithMany(a => a.UserTrainings)
            //    .HasForeignKey(u => u.UserFasserlyId);

            //modelBuilder.Entity<UserTraining>()
            //    .HasOne(t => t.Training)
            //    .WithMany(ut => ut.UserTrainings)
            //    .HasForeignKey(t => t.TrainingId);
        }
    }
}
