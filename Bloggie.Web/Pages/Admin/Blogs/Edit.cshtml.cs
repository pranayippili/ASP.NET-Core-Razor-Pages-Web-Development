using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Bloggie.Web.Pages.Admin.Blogs
{
	[Authorize(Roles = "Admin")]
	public class EditModel : PageModel
	{
		private readonly IBlogPostRepository blogPostRepository;

		[BindProperty]
		public EditBlogPostRequest BlogPost { get; set; }

		[BindProperty]
		public IFormFile FeaturedImage { get; set; }

		[BindProperty]
		[Required]
		public string Tags { get; set; }

		public EditModel(IBlogPostRepository blogPostRepository)
		{
			this.blogPostRepository = blogPostRepository;
		}
		public async Task OnGet(Guid id)
		{
			var blogPostDomainModel = await blogPostRepository.GetAsync(id);

			if (blogPostDomainModel != null)
			{
				BlogPost = new EditBlogPostRequest
				{
					Id = blogPostDomainModel.Id,
					Heading = blogPostDomainModel.Heading,
					PageTitle = blogPostDomainModel.PageTitle,
					Content = blogPostDomainModel.Content,
					ShortDescription = blogPostDomainModel.ShortDescription,
					FeaturedImageUrl = blogPostDomainModel.FeaturedImageUrl,
					UrlHandle = blogPostDomainModel.UrlHandle,
					PublishedDate = blogPostDomainModel.PublishedDate,
					Author = blogPostDomainModel.Author,
					Visible = blogPostDomainModel.Visible
				};
				if (blogPostDomainModel.Tags != null)
				{
					Tags = string.Join(",", blogPostDomainModel.Tags.Select(x => x.Name));
				}
				else
				{
					Tags = string.Empty; // Initialize Tags if BlogPost is null or has no tags
				}
			}

		}

		public async Task<IActionResult> OnPostEdit()
		{
			ValidateEditBLogPost();

			if (ModelState.IsValid)
			{
				try
				{
					var blogPostDomainModel = new BlogPost
					{
						Id = BlogPost.Id,
						Heading = BlogPost.Heading,
						PageTitle = BlogPost.PageTitle,
						Content = BlogPost.Content,
						ShortDescription = BlogPost.ShortDescription,
						FeaturedImageUrl = BlogPost.FeaturedImageUrl,
						UrlHandle = BlogPost.UrlHandle,
						PublishedDate = BlogPost.PublishedDate,
						Author = BlogPost.Author,
						Visible = BlogPost.Visible,
						Tags = new List<Tag>(Tags.Split(',').Select(x => new Tag() { Name = x.Trim() }))
					};


					await blogPostRepository.UpdateAsync(blogPostDomainModel);
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
			return Page(); // Return the current page if the model state is invalid

		}

		public async Task<IActionResult> OnPostDelete()
		{
			var deleted = await blogPostRepository.DeleteAsync(BlogPost.Id);
			if (deleted)
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

		private void ValidateEditBLogPost()
		{
			if (!string.IsNullOrWhiteSpace(BlogPost.Heading))
			{
				if (BlogPost.Heading.Length < 10 || BlogPost.Heading.Length > 72)
				{
					ModelState.AddModelError("BlogPost.Heading", "Heading must be between 10 and 72 characters.");
				}
			}
		}

	}
}
