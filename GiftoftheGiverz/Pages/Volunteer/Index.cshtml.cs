using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftoftheGiverz.Pages.Volunteer
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GiftoftheGiverz.Models.Volunteer Volunteer { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Volunteer.RegisteredDate = DateTime.Now;

            _context.Volunteers.Add(Volunteer);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Volunteer/Success");
        }
    }
}