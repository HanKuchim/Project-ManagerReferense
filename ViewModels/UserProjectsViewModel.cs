using Project_Manager.Models;

namespace Project_Manager.ViewModels
{
    public class UserProjectsViewModel
    {
        public List<Project> OwnedProjects { get; set; }
        public List<ProjectMember> InvitedProjects { get; set; }
    }
}
