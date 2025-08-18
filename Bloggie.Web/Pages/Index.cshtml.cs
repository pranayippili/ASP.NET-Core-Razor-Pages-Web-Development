using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages
{
    public class IndexModel(ILogger<IndexModel> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository) : PageModel
    {
		public List<BlogPost> Blogs { get; set; }
        public List<Tag> Tags { get; set; }

		public async Task<IActionResult> OnGet()
        {
            Blogs = (await blogPostRepository.GetAllAsync()).ToList();
            Tags = (await tagRepository.GetAllTagsAsync()).ToList();
			return Page();

        }
    }
}
