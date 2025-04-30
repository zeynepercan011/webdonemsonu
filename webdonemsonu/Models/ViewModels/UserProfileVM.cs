using System.ComponentModel.DataAnnotations;

namespace webdonemsonu.Models.ViewModels
{
	public class UserProfileVM
	{
		[Required]
		[Display(Name = "Ad")]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Soyad")]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "E-posta")]
		public string Email { get; set; }

		[Phone]
		[Display(Name = "Telefon")]
		public string? PhoneNumber { get; set; }

		[Display(Name = "Adres")]
		public string? Address { get; set; }
		public List<OrderVM> Orders { get; set; } = new();
	}
}