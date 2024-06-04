using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Models;
using Project_Manager.ViewModels;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class TaskAssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskAssignmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TaskAssignment/Assign
        //[Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Assign(int taskId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                return NotFound();
            }

            var projectMembers = await _context.ProjectMembers
                .Where(pm => pm.ProjectId == task.ProjectId)
                .Include(pm => pm.User)
                .ToListAsync();

            var viewModel = new TaskAssignViewModel
            {
                TaskId = taskId,
                ProjectId = task.ProjectId,
                Members = projectMembers.Select(pm => new SelectListItem
                {
                    Value = pm.Id.ToString(),
                    Text = pm.User.UserName
                }).ToList()
            };

            var userRole = await GetUserRoleInProject(task.ProjectId);
            ViewBag.UserRole = userRole?.Name;

            return View(viewModel);
        }

        // POST: TaskAssignment/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Assign(TaskAssignViewModel model)
        {
            //add model validation
            var taskAssignment = new TaskAssignment
            {
                TaskId = model.TaskId,
                ProjectMemberId = model.SelectedMemberId
            };

            _context.TaskAssignments.Add(taskAssignment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Task", new { id = model.TaskId });

            return View(model);
        }

        // POST: TaskAssignment/RemoveAssignment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAssignment(int taskId, int assignmentId)
        {
            var assignment = await _context.TaskAssignments
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null)
            {
                return NotFound();
            }

            _context.TaskAssignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Task", new { id = taskId });
        }

        private async Task<ProjectRole> GetUserRoleInProject(int projectId)
        {
            var userId = _userManager.GetUserId(User);
            var projectMember = await _context.ProjectMembers
                .Include(pm => pm.Role)
                .FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

            return projectMember?.Role;
        }
    }
}
