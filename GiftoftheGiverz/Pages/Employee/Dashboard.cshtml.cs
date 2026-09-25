using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftoftheGiverz.Pages.Employee
{
    [Authorize(Roles = "Employee")]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}