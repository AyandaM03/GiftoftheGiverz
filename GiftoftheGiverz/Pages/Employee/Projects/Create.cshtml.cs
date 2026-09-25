using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftoftheGiverz.Pages.Employee.Projects
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
        public ReliefProject ReliefProject { get; set; } = new();

        public void OnGet()
        {
            ReliefProject.StartDate = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ReliefProject.CreatedDate = DateTime.Now;

            _context.ReliefProjects.Add(ReliefProject);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}