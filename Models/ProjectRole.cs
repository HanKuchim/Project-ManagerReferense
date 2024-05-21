namespace Project_Manager.Models
{
    public class ProjectRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; }

    }
}
