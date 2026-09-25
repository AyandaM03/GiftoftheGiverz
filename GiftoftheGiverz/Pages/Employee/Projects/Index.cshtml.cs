using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiftoftheGiverz.Pages.Employee.Projects
{
    [Authorize(Roles = "Employee")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ReliefProject> Projects { get; set; }
            = new List<ReliefProject>();

        public async Task OnGetAsync()
        {
            Projects = await _context.ReliefProjects
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }
    }
}