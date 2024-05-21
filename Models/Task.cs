namespace Project_Manager.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int? AssignedMemberId { get; set; }
        public ProjectMember AssignedMember { get; set; }
        public int? ParentTaskId { get; set; }
        public Task ParentTask { get; set; }
        public ICollection<Task> SubTasks { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
