using Project_Manager.Models;

namespace Project_Manager.ViewModels
{
    public class ProjectInvitationListViewModel
    {
        public int ProjectId { get; set; }
        public List<ProjectInvitation> Invitations { get; set; }
    }
}
