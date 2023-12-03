using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Exam_System.Models;
using System;

namespace Online_Exam_System.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(SeedRoles());
            builder.Entity<User>().HasData(SeedSuperAdmin());
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1", // RoleId for SuperAdmin
                    UserId = "1" // User Id for the default user
                }
            );

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        private List<IdentityRole> SeedRoles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Instructor", NormalizedName = "Instructor" },
                new IdentityRole { Id = "3", Name = "Student", NormalizedName = "Student" }
            };
        }

        private User SeedSuperAdmin()
        {
            var hasher = new PasswordHasher<User>();
            return new User
            {
                Id = "1",
                UserName = "TemporaryUsername",
                NormalizedUserName = "TEMPORARY-USERNAME",
                Email = "TemporaryEmail@example.com",
                NormalizedEmail = "TEMPORARYEMAIL@EXAMPLE.COM",
                name = "Temporary first Name",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "TemporaryPassword"),
                SecurityStamp = string.Empty
            };
        }
    }
}
