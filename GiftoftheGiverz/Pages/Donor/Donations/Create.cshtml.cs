using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace GiftoftheGiverz.Pages.Donor.Donations
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly HttpClient _httpClient;

        public CreateModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _userManager = userManager;
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public Donation Donation { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Donation.DonationType == "Recurring" &&
                string.IsNullOrWhiteSpace(Donation.RecurringFrequency))
            {
                ModelState.AddModelError(
                    "Donation.RecurringFrequency",
                    "Please select a recurring frequency.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // If the user is logged in, associate the donation
            // with their Identity account.
            if (User.Identity?.IsAuthenticated == true)
            {
                Donation.UserId = _userManager.GetUserId(User);
            }
            else
            {
                // Anonymous guest donation
                Donation.UserId = null;
                Donation.IsAnonymous = true;
            }

            Donation.DonationDate = DateTime.Now;

            Donation.Status = "Confirmed";

            Donation.ReferenceNumber =
                $"GOG-{DateTime.Now:yyyyMMddHHmmss}-{Random.Shared.Next(100, 999)}";

            // Save the donation to the database
            _context.Donations.Add(Donation);

            await _context.SaveChangesAsync();

            // Get the donor's name
            string donorName;

            if (Donation.IsAnonymous)
            {
                donorName = "Anonymous Donor";
            }
            else
            {
                donorName = User.Identity?.Name ?? "Registered Donor";
            }

            // Prepare the information that will be sent
            // to the Azure Function.
            var donationRequest = new
            {
                DonorName = donorName,
                Amount = Donation.Amount,
                Currency = Donation.Currency,
                ReferenceNumber = Donation.ReferenceNumber,
                DonationType = Donation.DonationType
            };

            try
            {
                // Call the local Azure Function.
                var functionUrl =
                    "http://localhost:7131/api/GenerateTaxCertificate";

                var functionResponse =
                    await _httpClient.PostAsJsonAsync(
                        functionUrl,
                        donationRequest);

                if (functionResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        "Tax certificate generated successfully by Azure Function.");
                }
                else
                {
                    Console.WriteLine(
                        $"Azure Function returned status code: {functionResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Could not connect to the Azure Function: {ex.Message}");
            }

            return RedirectToPage(
                "/Donor/Donations/Confirmation",
                new { id = Donation.DonationId });
        }
    }
}