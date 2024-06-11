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
        public DbSet<ProjectInvitation> ProjectInvitations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskAssignment>()
                .HasKey(ta => new { ta.TaskId, ta.ProjectMemberId });

            
            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Task)
                .WithMany(t => t.TaskAssignments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.ProjectMember)
                .WithMany(pm => pm.TaskAssignments)
                .HasForeignKey(ta => ta.ProjectMemberId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Task>()
                .HasOne(t => t.ParentTask)
                .WithMany(t => t.SubTasks)
                .HasForeignKey(t => t.ParentTaskId)
                .OnDelete(DeleteBehavior.Restrict); 

            
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany()
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<ProjectRole>()
                .HasMany(pr => pr.ProjectMembers)
                .WithOne(pm => pm.Role)
                .HasForeignKey(pm => pm.RoleId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ProjectInvitation>()
               .HasOne(pi => pi.Project)
               .WithMany(p => p.ProjectInvitations)
               .HasForeignKey(pi => pi.ProjectId);

            modelBuilder.Entity<ProjectInvitation>()
                .HasOne(pi => pi.Sender)
                .WithMany(u => u.SentInvitations)
                .HasForeignKey(pi => pi.SenderId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ProjectInvitation>()
                .HasOne(pi => pi.Recipient)
                .WithMany(u => u.ReceivedInvitations)
                .HasForeignKey(pi => pi.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectRole>().HasData(
                new ProjectRole { Id = 1, Name = "Admin" },
                new ProjectRole { Id = 2, Name = "Moderator" },
                new ProjectRole { Id = 3, Name = "User" });
        }
    }
}
