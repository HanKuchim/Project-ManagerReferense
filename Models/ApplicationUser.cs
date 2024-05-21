using Microsoft.AspNetCore.Identity;

namespace Project_Manager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Project> Projects { get; set; }
    }
}
