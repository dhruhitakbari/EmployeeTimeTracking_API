using EmployeeTimeTracking_API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Repository.Data
{
    public class AppDbContext : DbContext
    {   
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        // --- Register your models as tables ---
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimeLog> TimeLogs { get; set; }
        public DbSet<UserProject> UserProjects { get; set; } // The junction table

        // --- THIS IS WHERE YOU DEFINE RELATIONSHIPS ---
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Seed Roles ---
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "Admin" },
                new Role { RoleId = 2, Name = "Employee" }
            );

            // --- 1. One-to-Many Relationships ---
            // (e.g., User and Role)
            // "One Role has Many Users"
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role) // A User has one Role
                .WithMany(r => r.Users) // A Role has many Users
                .HasForeignKey(u => u.RoleId); // The foreign key is RoleId

            // (e.g., User and Department)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .IsRequired(false); // We make it optional (nullable)

            // Relationship 2: User to Designation (assuming you have a Designation class)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Designation)
                .WithMany(d => d.Users) // Assuming Designation has a 'Users' list
                .HasForeignKey(u => u.DesignationId)
                .IsRequired(false);

            // (e.g., User and TimeLog)
            modelBuilder.Entity<TimeLog>()
                .HasOne(tl => tl.User)
                .WithMany(u => u.TimeLogs)
                .HasForeignKey(tl => tl.UserId);


            // --- 2. Many-to-Many Relationship (User and Project) ---
            // This requires configuring the junction table 'UserProject'

            // First, define the composite primary key for UserProject
            modelBuilder.Entity<UserProject>()
                .HasKey(up => new { up.UserId, up.ProjectId });

            // Set up the relationship from User -> UserProject
            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId);

            // Set up the relationship from Project -> UserProject
            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId);
        }
    }
}
