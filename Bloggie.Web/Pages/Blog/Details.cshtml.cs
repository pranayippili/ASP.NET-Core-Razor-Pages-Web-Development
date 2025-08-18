using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Pages.Blog
{
	public class DetailsModel(IBlogPostRepository blogPostRepository,
		IBlogPostLikeRepository blogPostLikeRepository,
		SignInManager<IdentityUser> signInManager,
		UserManager<IdentityUser> userManager,
		IBlogPostCommentRepository blogPostCommentRepository) : PageModel
	{
		public BlogPost BlogPost { get; set; }

		public List<BlogComment> Comments { get; set; }

		public int TotalLikes { get; set; }

		public bool Liked { get; set; }

		[BindProperty]
		public Guid BlogPostId { get; set; }

		[BindProperty]
		[Required]
		[MinLength(1)]
		[MaxLength(200)]
		public string CommentDescription { get; set; }

		public async Task<IActionResult> OnGet(string urlHandle)
        {
			await GetBlog(urlHandle);

			return Page();

		}

		public async Task<IActionResult> OnPost(string urlHandle)
		{
			if (ModelState.IsValid)
			{
				if (signInManager.IsSignedIn(User) && !string.IsNullOrWhiteSpace(CommentDescription))
				{
					var userId = Guid.Parse(userManager.GetUserId(User));
					var comment = new BlogPostComment
					{
						BlogPostId = BlogPostId,
						Description = CommentDescription,
						DateAdded = DateTime.Now,
						UserId = userId
					};
					await blogPostCommentRepository.AddAsync(comment);
				}
				return RedirectToPage("/Blog/Details", new { urlHandle = urlHandle });
			}
			await GetBlog(urlHandle);
			return Page();
		}

		private async Task GetComments()
		{
			var blogPostComments = await blogPostCommentRepository.GetAllAsync(BlogPost.Id);

			var blogCommentsViewModel = new List<BlogComment>();

			foreach (var blogPostComment in blogPostComments)
			{
				blogCommentsViewModel.Add(new BlogComment
				{
					DateAdded = blogPostComment.DateAdded,
					Description = blogPostComment.Description,
					UserName = (await userManager.FindByIdAsync(blogPostComment.UserId.ToString())).UserName
				});
			}

			Comments = blogCommentsViewModel;
		}

		private async Task GetBlog(string urlHandle)
		{
			BlogPost = await blogPostRepository.GetAsync(urlHandle);

			if (BlogPost != null)
			{
				BlogPostId = BlogPost.Id;
				if (signInManager.IsSignedIn(User))
				{
					var likes = await blogPostLikeRepository.GetLikesForBlog(BlogPost.Id);

					var userId = userManager.GetUserId(User);

					Liked = likes.Any(x => x.UserId == Guid.Parse(userId));

					await GetComments();
				}


				TotalLikes = await blogPostLikeRepository.GetTotalLikesForBlog(BlogPost.Id);
			}
		}
    }
}
