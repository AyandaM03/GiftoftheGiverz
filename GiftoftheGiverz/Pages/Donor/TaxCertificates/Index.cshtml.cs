using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiftoftheGiverz.Pages.Donor.TaxCertificates
{
    [Authorize(Roles = "Donor")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public TaxCertificate? Certificate { get; set; }

        public Donation? Donation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? donationId)
        {
            if (donationId == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            Donation = await _context.Donations
                .FirstOrDefaultAsync(d =>
                    d.DonationId == donationId &&
                    d.UserId == userId);

            if (Donation == null)
            {
                return NotFound();
            }

            Certificate = await _context.TaxCertificates
                .FirstOrDefaultAsync(t =>
                    t.DonationId == Donation.DonationId);

            // Generate a placeholder certificate if one
            // does not already exist.
            if (Certificate == null)
            {
                Certificate = new TaxCertificate
                {
                    DonationId = Donation.DonationId,
                    CertificateNumber =
                        $"TAX-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
                    IssuedDate = DateTime.Now,
                    IssuedBy = "Gift of the Givers",
                    PdfLink = null
                };

                _context.TaxCertificates.Add(Certificate);

                await _context.SaveChangesAsync();
            }

            return Page();
        }
    }
}