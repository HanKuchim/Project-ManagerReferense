namespace Project_Manager.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
        public int ProjectMemberId { get; set; }
        public ProjectMember ProjectMember { get; set; }
    }
}
