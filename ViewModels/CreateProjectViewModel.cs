using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels
{
    public class CreateProjectViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

    }
}
