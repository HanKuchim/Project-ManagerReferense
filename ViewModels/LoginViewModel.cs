using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name ="Remember me?")]
        public bool RememberMe { get; set; }
    }
}
