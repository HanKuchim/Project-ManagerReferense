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
    public class ProjectInvitationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectInvitationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create(int projectId)
        {
            var viewModel = new ProjectInvitationCreateViewModel
            {
                ProjectId = projectId
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectInvitationCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var recipient = await _userManager.FindByNameAsync(model.RecipientIdentifier) ?? await _userManager.FindByEmailAsync(model.RecipientIdentifier);

            if (recipient == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View(model);
            }

            var invitation = new ProjectInvitation
            {
                ProjectId = model.ProjectId,
                SenderId = _userManager.GetUserId(User),
                RecipientId = recipient.Id,
                SentDate = DateTime.UtcNow,
                Status = InvitationStatus.Pending
            };

            _context.ProjectInvitations.Add(invitation);
            await _context.SaveChangesAsync();

            //return RedirectToAction ("Index", "ProjectMembers", model.ProjectId);
            //return Redirect($"ProjectMembers/Index?ProjectId={model.ProjectId}");
            var url = Url.Action("Index", "ProjectMembers", new { projectId = model.ProjectId });
            return Redirect(url);
        }
        [HttpGet]
        public async Task<IActionResult> Invitations(int projectId)
        {
            var invitations = await _context.ProjectInvitations
                 .Where(i => i.ProjectId == projectId)
                 .Include(i => i.Sender)
                 .Include(i => i.Recipient)
                 .ToListAsync();

            var viewModel = new ProjectInvitationListViewModel
            {
                ProjectId = projectId,
                Invitations = invitations
            };

            return View(viewModel);
        }
    }
}
