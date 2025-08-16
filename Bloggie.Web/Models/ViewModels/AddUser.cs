using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
	public class AddUser
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(6)]
		public string Password { get; set; }
		public bool AdminCheckBox { get; set; }
	}
}
