using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiftoftheGiverz.Pages.Employee.Volunteers
{
    [Authorize(Roles = "Employee")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<GiftoftheGiverz.Models.Volunteer> Volunteers { get; set; }
            = new List<GiftoftheGiverz.Models.Volunteer>();

        public async Task OnGetAsync()
        {
            Volunteers = await _context.Volunteers
                .OrderByDescending(v => v.RegisteredDate)
                .ToListAsync();
        }
    }
}