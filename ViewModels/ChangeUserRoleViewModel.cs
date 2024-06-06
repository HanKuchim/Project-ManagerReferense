using Project_Manager.Models;

namespace Project_Manager.ViewModels
{
    public class ChangeUserRoleViewModel
    {
        public int ProjectId { get; set; }
        public ProjectMember ProjectMember { get; set; }
        public IEnumerable<ProjectRole> ProjectRoles { get; set; }
    }
}
