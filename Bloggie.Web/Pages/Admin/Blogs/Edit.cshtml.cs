using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Bloggie.Web.Pages.Admin.Blogs
{
    public class EditModel : PageModel
    {
		private readonly IBlogPostRepository blogPostRepository;

		[BindProperty]
		public BlogPost BlogPost { get; set; }

		[BindProperty]
		public IFormFile FeaturedImage { get; set; }

		public EditModel(IBlogPostRepository blogPostRepository)
		{
			this.blogPostRepository = blogPostRepository;
		}
		public async Task OnGet(Guid id)
        {
			BlogPost = await blogPostRepository.GetAsync(id);
		}

		public async Task<IActionResult> OnPostEdit() 
		{
			try
			{
				await blogPostRepository.UpdateAsync(BlogPost);
				ViewData["Notification"] = new Notification
				{
					Message = "Blog post updated successfully!",
					Type = Enums.NotificationType.Success
				};
			}
			catch (Exception ex)
			{
				ViewData["Notification"] = new Notification
				{
					Message = "Something went wrong!",
					Type = Enums.NotificationType.Error
				};
			}

			return Page();
		}

		public async Task<IActionResult> OnPostDelete()
		{
			var deleted = await blogPostRepository.DeleteAsync(BlogPost.Id);
			if(deleted)
			{
				var notification = new Notification
				{
					Type = Enums.NotificationType.Success,
					Message = "Blog was deleted successfully!"
				};

				TempData["Notification"] = JsonSerializer.Serialize(notification);
				return RedirectToPage("/Admin/Blogs/List");
			}

			return Page(); // Return the current page if the blog post was not found
		}

	}
}
