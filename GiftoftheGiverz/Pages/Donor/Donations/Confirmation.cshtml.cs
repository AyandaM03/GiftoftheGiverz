using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiftoftheGiverz.Pages.Donor.Donations
{
    public class ConfirmationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ConfirmationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Donation? Donation { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.DonationId == id);

            if (Donation == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}