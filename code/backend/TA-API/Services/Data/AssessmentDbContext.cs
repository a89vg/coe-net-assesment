using Microsoft.EntityFrameworkCore;
using TA_API.Models.Data;

namespace TA_API.Services.Data
{
    public class AssessmentDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public AssessmentDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Username = "admin", FullName = "Alejandro Vázquez Góngora", Email = "admin@unosquare.com", DateOfBirth = new DateTime(1989, 5, 25), PasswordHash = "AQAAAAIAAYagAAAAEPqCWJ7ya9WKGosM3zc8YI8gLojDSX+psFE9t73BstHwFT5XwOFFlTQwC65YvoK0sQ==" },
                new User() { Id = 2, Username = "avazquez", FullName = "Alejandro Vázquez Gógnora", Email = "alejandro.vazquez@unosquare.com", DateOfBirth = new DateTime(1989, 5, 25), PasswordHash = "AQAAAAIAAYagAAAAEEOCuWEdhDbhYbrCSiQmdXZG9ZZHq1bcDo4Ku5klEDg8TkTpma+DciN19NO8dzlA6w==" }
            );

            modelBuilder.Entity<Role>().ToTable("Roles").HasKey(r => r.Id);
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            modelBuilder.Entity<UserRole>().ToTable("UserRoles").HasIndex(nameof(UserRole.Username));
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, Username = "admin", Role = "Admin" },
                new UserRole { Id = 2, Username = "avazquez", Role = "User" }
            );
        }
    }
}
