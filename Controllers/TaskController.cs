using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public TaskController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                .Include(t => t.SubTasks)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }
        private async Task<ProjectRole> GetUserRoleInProject(int projectId)
        {
            var userId = _userManager.GetUserId(User);
            var projectMember = await _context.ProjectMembers
                .Include(pm => pm.Role)
                .FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

            return projectMember?.Role;
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
    }
}

