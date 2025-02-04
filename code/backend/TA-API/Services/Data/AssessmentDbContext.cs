using Microsoft.EntityFrameworkCore;
using TA_API.Models;

namespace TA_API.Services.Data
{
    public class AssessmentDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AssessmentDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
        }


    }
}
