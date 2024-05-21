namespace Project_Manager.Models
{
    public class ProjectMember
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int RoleId { get; set; }
        public ProjectRole Role { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
    }
}
