using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace webdonemsonu.Models
{
	public class ApplicationUser : IdentityUser //emailmpassword hazır geliyor.
		//IdentityUserden miras aldığımz için.
	{
		[Required]
		[PersonalData]
		public string FirstName { get; set; }

		[Required]
		[PersonalData]
		public string LastName { get; set; }

		[PersonalData]
		public string? Address { get; set; }
		public virtual ICollection<Order>? Orders { get; set; }
	}
}
