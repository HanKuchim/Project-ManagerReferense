using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ProjectMembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectMembersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<ProjectRole> GetUserRoleInProject(int projectId)
        {
            var userId = _userManager.GetUserId(User);
            var projectMember = await _context.ProjectMembers
                .Include(pm => pm.Role)
                .FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

            return projectMember?.Role;
        }

        public async Task<IActionResult> Index(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectMembers)
                .ThenInclude(pm => pm.User)
                .Include(p => p.ProjectMembers)
                .ThenInclude(pm => pm.Role)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return NotFound();
            }

            var userRole = await GetUserRoleInProject(projectId);
            ViewBag.UserRole = userRole?.Name;

            return View(project);
        }
    }
}
