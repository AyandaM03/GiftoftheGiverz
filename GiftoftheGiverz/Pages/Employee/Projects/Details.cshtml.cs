using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiftoftheGiverz.Pages.Employee.Projects
{
    [Authorize(Roles = "Employee")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public ReliefProject? Project { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Project = await _context.ReliefProjects
                .Include(p => p.Updates)
                .FirstOrDefaultAsync(
                    p => p.ReliefProjectId == id);

            if (Project == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}