using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bloggie.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ImagesController : Controller
	{
		private readonly IImageRespository imageRespository;
		public ImagesController(IImageRespository imageRespository)
		{
			this.imageRespository = imageRespository;
		}

		[HttpPost]
		public async Task<IActionResult> UploadAsync(IFormFile file)
		{
			var imageUrl = await imageRespository.UploadAsync(file);

			if (string.IsNullOrEmpty(imageUrl))
			{
				return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
			}
			return Json(new { link = imageUrl });

		}

	}
}
