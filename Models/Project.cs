namespace Project_Manager.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; }
        public ICollection<ProjectInvitation> ProjectInvitations { get; set; }

    }
}
