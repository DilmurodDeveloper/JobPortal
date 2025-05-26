using JobPortal.Api.Models.Foundations.Applications;
using JobPortal.Api.Models.Foundations.JobPosts;
using JobPortal.Api.Models.Foundations.UserProfiles;
using JobPortal.Api.Models.Foundations.Users;

namespace JobPortal.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<JobPost> JobPosts { get; set; } = null!;
        public DbSet<Application> Applications { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<JobPost>()
                .HasOne<User>()              
                .WithMany()  
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
