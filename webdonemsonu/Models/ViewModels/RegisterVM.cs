using System.ComponentModel.DataAnnotations;

namespace webdonemsonu.Models.ViewModels
{
	public class RegisterVM
	{
		[Required(ErrorMessage = "Ad alanı zorunludur.")]
		[Display(Name = "Ad")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Soyad alanı zorunludur.")]
		[Display(Name = "Soyad")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "E-posta zorunludur.")]
		[EmailAddress(ErrorMessage = "Geçersiz e-posta formatı.")]
		[Display(Name = "E-posta")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre zorunludur.")]
		[DataType(DataType.Password)]
		[StringLength(100, ErrorMessage = "Şifre en az {2} karakter olmalıdır.", MinimumLength = 6)]
		[Display(Name = "Şifre")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Şifre Tekrar")]
		[Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
		public string ConfirmPassword { get; set; }
	}
}