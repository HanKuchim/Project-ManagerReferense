using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Models;
using Project_Manager.ViewModels;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var ownedProjects = await _context.Projects
                .Where(p => p.OwnerId == user.Id)
                .ToListAsync();

            var invitedProjects = await _context.ProjectMembers
                .Include(pm => pm.Project)
                .ThenInclude(p => p.Owner)
                .Include(pm => pm.Role)
                .Where(pm => pm.UserId == user.Id && pm.Project.OwnerId != user.Id)
                .ToListAsync();

            var viewModel = new UserProjectsViewModel
            {
                OwnedProjects = ownedProjects,
                InvitedProjects = invitedProjects
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var project = new Project
                {
                    Title = model.Title,
                    Description = model.Description,
                    CreatedDate = DateTime.Now,
                    OwnerId = user.Id,
                    Owner = user
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                var projectMember = new ProjectMember
                {
                    ProjectId = project.Id,

                    UserId = user.Id,

                    RoleId = 1
                };

                _context.ProjectMembers.Add(projectMember);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Project");
            }
            return View(model);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Tasks)
                .ThenInclude(t => t.SubTasks)
                .Include(p => p.ProjectMembers)
                .ThenInclude(pm => pm.Role)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            var userRole = await GetUserRoleInProject(id);
            ViewBag.UserRole = userRole?.Name;  // Pass the role name to the view


            return View(project);

        }

        // GET: Projects/Manage/5
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            var userRole = await GetUserRoleInProject(project.Id);
            if (userRole?.Name != "Admin")
            {
                return Forbid();
            }

            return View(project);
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.ProjectMembers)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
