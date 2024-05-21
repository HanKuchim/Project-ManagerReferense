using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Models;
using Task = Project_Manager.Models.Task;

namespace Project_Manager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>()
               .HasOne(p => p.Owner)
               .WithMany(u => u.Projects)  // Обновлено для указания коллекции проектов у пользователя
               .HasForeignKey(p => p.OwnerId)
               .OnDelete(DeleteBehavior.Restrict); // Устанавливаем поведение при удалении, чтобы избежать циклических зависимостей


            builder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId);

            builder.Entity<Project>()
                .HasMany(p => p.ProjectMembers)
                .WithOne(pm => pm.Project)
                .HasForeignKey(pm => pm.ProjectId);

            builder.Entity<Task>()
                .HasMany(t => t.SubTasks)
                .WithOne(t => t.ParentTask)
                .HasForeignKey(t => t.ParentTaskId);

            builder.Entity<Task>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId);

            builder.Entity<ProjectMember>()
                .HasOne(pm => pm.Role)
                .WithMany(r => r.ProjectMembers)
                .HasForeignKey(pm => pm.RoleId);

            builder.Entity<ProjectMember>()
                .HasMany(pm => pm.AssignedTasks)
                .WithOne(t => t.AssignedMember)
                .HasForeignKey(t => t.AssignedMemberId);
        }
    }
}
