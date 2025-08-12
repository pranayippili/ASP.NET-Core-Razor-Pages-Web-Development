using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages
{
    public class LoginModel : PageModel
    {
		private readonly SignInManager<IdentityUser> signInManager;

		[BindProperty]
		public Login LoginViewModel { get; set; }

		public LoginModel(SignInManager<IdentityUser> signInManager)
		{
			this.signInManager = signInManager;
		}
		public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost(string ReturnUrl) 
        {
			var signINResult = await signInManager.PasswordSignInAsync(
				LoginViewModel.Username, LoginViewModel.Password, false, false);

			if (signINResult.Succeeded)
			{
				if (!string.IsNullOrWhiteSpace(ReturnUrl))
				{
					return RedirectToPage(ReturnUrl);
				}
				return RedirectToPage("Index");
			}
			else 
			{
				ViewData["Notification"] = new Notification
				{
					Type = Enums.NotificationType.Error,
					Message = "Login failed. Please check your username and password."
				};
				return Page();
			}
		}
    }
}
