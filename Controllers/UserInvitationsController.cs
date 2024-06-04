using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Models;

namespace Project_Manager.Controllers
{
    public class UserInvitationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserInvitationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var invitations = await _context.ProjectInvitations
                .Include(pi => pi.Project)
                .Include(pi => pi.Sender)
                .Include(pi => pi.Recipient)
                .Where(pi => pi.RecipientId == userId && pi.Status == InvitationStatus.Pending)
                .ToListAsync();

            return View(invitations);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var invitation = await _context.ProjectInvitations.FindAsync(id);

            if (invitation == null || invitation.RecipientId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            invitation.Status = InvitationStatus.Accepted;

            var projectMember = new ProjectMember
            {
                ProjectId = invitation.ProjectId,
                UserId = invitation.RecipientId,
                RoleId = 3 // RoleId for 'User'
            };

            _context.ProjectMembers.Add(projectMember);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Decline(int id)
        {
            var invitation = await _context.ProjectInvitations.FindAsync(id);

            if (invitation == null || invitation.RecipientId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            invitation.Status = InvitationStatus.Declined;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
