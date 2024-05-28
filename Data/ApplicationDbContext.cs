using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Models;
using Task = Project_Manager.Models.Task;

namespace Project_Manager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define composite keys for TaskAssignment
            modelBuilder.Entity<TaskAssignment>()
                .HasKey(ta => new { ta.TaskId, ta.ProjectMemberId });

            // Configure relationships for TaskAssignment
            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Task)
                .WithMany(t => t.TaskAssignments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict or NoAction to prevent multiple cascade paths

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.ProjectMember)
                .WithMany(pm => pm.TaskAssignments)
                .HasForeignKey(ta => ta.ProjectMemberId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict or NoAction to prevent multiple cascade paths

            // Configure relationships for Task
            modelBuilder.Entity<Task>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); // If you want to cascade delete tasks when a project is deleted

            modelBuilder.Entity<Task>()
                .HasOne(t => t.ParentTask)
                .WithMany(t => t.SubTasks)
                .HasForeignKey(t => t.ParentTaskId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict or NoAction to prevent multiple cascade paths

            // Configure relationships for Project
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict or NoAction to prevent multiple cascade paths

            // Configure relationships for ProjectMember
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); // If you want to cascade delete project members when a project is deleted

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany()
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict or NoAction to prevent multiple cascade paths

            // Configure relationships for ProjectRole
            modelBuilder.Entity<ProjectRole>()
                .HasMany(pr => pr.ProjectMembers)
                .WithOne(pm => pm.Role)
                .HasForeignKey(pm => pm.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict or NoAction to prevent multiple cascade paths
        }
    }
}
