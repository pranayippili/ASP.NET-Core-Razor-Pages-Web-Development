using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages
{
    public class LogoutModel(SignInManager<IdentityUser> signInManager) : PageModel
    {
		public async Task<IActionResult> OnGet()
        {
			await signInManager.SignOutAsync();
			return RedirectToPage("Index");
		}

    }
}
