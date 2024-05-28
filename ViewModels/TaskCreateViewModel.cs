using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels
{
    public class TaskCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        [Range(1, 10)]
        public int Priority { get; set; }

        public int ProjectId { get; set; }

        public int? ParentTaskId { get; set; }
    }
}
