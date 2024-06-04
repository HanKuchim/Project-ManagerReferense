using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace Project_Manager.ViewModels
{
    public class TaskAssignViewModel
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        [AllowNull]
        public List<SelectListItem> Members { get; set; }
        [AllowNull]
        public int SelectedMemberId { get; set; }
    }
}
