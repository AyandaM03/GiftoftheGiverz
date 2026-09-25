using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftoftheGiverz.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
            {
                // Validation failed - redisplay the form with errors
                return Page();
            }

            var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };

            // Create the user with the given password
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("A new user account was created.");

                // Ensure the Donor role exists and add the new user to it.
                // This makes all public registrations Donors by default.
                // Employees must be created/assigned separately by an admin or developer.
                var donorRoleExists = await _roleManager.RoleExistsAsync("Donor");
                if (!donorRoleExists)
                {
                    // Role seeding runs at startup, but check again to be safe.
                    await _roleManager.CreateAsync(new IdentityRole("Donor"));
                }

                // Add the user to the Donor role
                await _userManager.AddToRoleAsync(user, "Donor");

                // Sign in the user if you want automatic sign-in; keep default Identity behavior.
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Redirect to return URL or homepage
                return LocalRedirect(ReturnUrl);
            }

            // If we get here, something failed - add errors to ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // If we reach here, redisplay the form
            return Page();
        }
    }
}