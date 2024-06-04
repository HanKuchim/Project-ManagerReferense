using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels
{
    public class ProjectInvitationCreateViewModel
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Recipient Username or Email")]
        public string RecipientIdentifier { get; set; }
    }
}
