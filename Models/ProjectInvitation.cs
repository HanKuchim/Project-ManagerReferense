using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Models
{
    public class ProjectInvitation
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        [Required]
        public string RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }

        [Required]
        public DateTime SentDate { get; set; }

        public DateTime? AcceptedDate { get; set; }
        public DateTime? DeclinedDate { get; set; }

        [Required]
        public InvitationStatus Status { get; set; }
    }

    public enum InvitationStatus
    {
        Pending,
        Accepted,
        Declined
    }
}
