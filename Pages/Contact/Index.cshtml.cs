using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftoftheGiverz.Pages.Contact
{
    public class IndexModel : PageModel
    {
        // Simple contact form input model with validation.
        public class ContactInputModel
        {
            [Required]
            [Display(Name = "Full name")]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email address")]
            public string Email { get; set; }

            [Required]
            [StringLength(100)]
            public string Subject { get; set; }

            [Required]
            [StringLength(2000, ErrorMessage = "The message must be 2000 characters or fewer.")]
            public string Message { get; set; }
        }

        // Bind the form input for POST handling
        [BindProperty]
        public ContactInputModel Input { get; set; }

        public void OnGet()
        {
            // Nothing to initialise for GET
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Validation failed - redisplay the form with validation messages
                return Page();
            }

            // In a real application we'd send an email or save the message.
            // For this prototype we simulate contact handling and display a success message.
            TempData["SuccessMessage"] = "Thank you. Your message has been received. We will respond shortly.";

            // Clear the model so the form appears empty after successful submit
            ModelState.Clear();
            Input = new ContactInputModel();

            return RedirectToPage(); // PRG pattern to avoid duplicate submissions
        }
    }