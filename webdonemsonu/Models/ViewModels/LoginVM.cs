using System.ComponentModel.DataAnnotations;

namespace webdonemsonu.Models.ViewModels
{
	public class LoginVM
	{
		[Required(ErrorMessage = "E-posta zorunludur.")]
		[EmailAddress]
		[Display(Name = "E-posta")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre zorunludur.")]
		[DataType(DataType.Password)]
		[Display(Name = "Şifre")]
		public string Password { get; set; }

		[Display(Name = "Beni Hatırla?")]
		public bool RememberMe { get; set; }
	}
}