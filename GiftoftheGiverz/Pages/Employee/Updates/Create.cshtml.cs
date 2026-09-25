using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GiftoftheGiverz.Pages.Employee.Updates
{
    [Authorize(Roles = "Employee")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProjectUpdate ProjectUpdate { get; set; } = new();

        public SelectList ReliefProjects { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadProjectsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Make sure the selected project actually exists.
            var projectExists = await _context.ReliefProjects
                .AnyAsync(p =>
                    p.ReliefProjectId ==
                    ProjectUpdate.ReliefProjectId);

            if (!projectExists)
            {
                ModelState.AddModelError(
                    "ProjectUpdate.ReliefProjectId",
                    "Please select a valid relief project.");
            }

            if (!ModelState.IsValid)
            {
                await LoadProjectsAsync();
                return Page();
            }

            ProjectUpdate.DatePosted = DateTime.Now;

            ProjectUpdate.PostedByUserId =
                User.Identity?.Name;

            _context.ProjectUpdates.Add(ProjectUpdate);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Employee/Projects/Details",
                new
                {
                    id = ProjectUpdate.ReliefProjectId
                });
        }

        private async Task LoadProjectsAsync()
        {
            var projects = await _context.ReliefProjects
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            ReliefProjects = new SelectList(
                projects,
                "ReliefProjectId",
                "Name");
        }
    }
}