using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftoftheGiverz.Pages.Contact
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // The contact form is currently a prototype.
            // The message is displayed as confirmation for now.

            TempData["Message"] =
                "Thank you for contacting us. Your message has been received.";

            return RedirectToPage();
        }
    }
}