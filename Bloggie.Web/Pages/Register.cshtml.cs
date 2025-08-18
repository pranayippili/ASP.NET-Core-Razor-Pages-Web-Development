using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages
{
	public class RegisterModel(UserManager<IdentityUser> userManager) : PageModel
	{
		[BindProperty]
		public Register RegisterViewModel { get; set; }

		public void OnGet()
		{
		}
		public async Task<IActionResult> OnPost()
		{
			if (ModelState.IsValid)
			{
				var user = new IdentityUser
				{
					UserName = RegisterViewModel.Username,
					Email = RegisterViewModel.Email
				};
				var identityResult = await userManager.CreateAsync(user, RegisterViewModel.Password);

				if (identityResult.Succeeded)
				{
					var addRolesResult = await userManager.AddToRoleAsync(user, "User");

					if (addRolesResult.Succeeded)
					{
						ViewData["Notification"] = new Notification
						{
							Type = Enums.NotificationType.Success,
							Message = "User registered successfully."
						};

						return Page();

					}
				}

				ViewData["Notification"] = new Notification
				{
					Type = Enums.NotificationType.Error,
					Message = "User registration failed. Please try again."
				};

				return Page();
			}
			else
			{
				return Page();
			}


		}
	}
}
