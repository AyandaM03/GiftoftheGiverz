using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiftoftheGiverz.Pages.Donor.Donations
{
    [Authorize(Roles = "Donor")]
    public class HistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HistoryModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Donation> Donations { get; set; }
            = new List<Donation>();

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            Donations = await _context.Donations
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.DonationDate)
                .ToListAsync();
        }
    }
}