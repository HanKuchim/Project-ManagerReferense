using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Models;
using Project_Manager.ViewModels;
using Task = Project_Manager.Models.Task;


namespace Project_Manager.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public TaskController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Tasks/Create
        //[Authorize(Roles = "Admin, Moderator")]
        public IActionResult Create(int projectId, int? parentTaskId)
        {
            var viewModel = new TaskCreateViewModel
            {
                ProjectId = projectId,
                ParentTaskId = parentTaskId
            };
            return View(viewModel);
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create(TaskCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var task = new Task
                {
                    Title = model.Title,
                    Description = model.Description,
                    DueDate = model.DueDate,
                    Priority = model.Priority,
                    Status = "New",
                    ProjectId = model.ProjectId,
                    ParentTaskId = model.ParentTaskId
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return Redirect($"Index?ProjectId={model.ProjectId}");
                //return RedirectToAction("Index",model.ProjectId);

            }
            return BadRequest("404");
            return View(model);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.TaskAssignments)
                .ThenInclude(ta => ta.ProjectMember)
                //добавь инклюд ролей
                .ThenInclude(pm => pm.User)
                .Include(t => t.TaskAssignments)
                .ThenInclude(ta => ta.ProjectMember)
                .ThenInclude(ta => ta.Role)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            var userRole = await GetUserRoleInProject(task.ProjectId);
            ViewBag.UserRole = userRole?.Name;

            return View(task);
        }

        

        public async Task<IActionResult> Index(int ProjectId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == ProjectId);

            if (project == null)
            {
                return NotFound();
            }

            var userRole = await GetUserRoleInProject(ProjectId);
            ViewBag.UserRole = userRole?.Name;

            return View(project);
        }

        // GET: Tasks/Delete/5
        //[Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            var userRole = await GetUserRoleInProject(task.ProjectId);
            if (userRole?.Name != "Admin" && userRole?.Name != "Moderator")
            {
                return Forbid();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.SubTasks)
                .Include(t => t.Comments)
                .Include(t => t.TaskAssignments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { ProjectId = task.ProjectId });
        }

        public IActionResult ChangeStatus(int taskId)
        {
            
            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (task == null)
            {
                return NotFound();
            }

            
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int taskId, string status)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                return NotFound();
            }

            task.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = taskId });
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

