using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace webdonemsonu.Models.ViewModels
{
	public class ProductVM
	{
		public int Id;
		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		[Range(0.01, double.MaxValue)]
		public decimal Price { get; set; }

		public byte[] Image { get; set; }

		[Required]
		[Display(Name = "Kategori")]
		public int CategoryId { get; set; }

		public string? CategoryName { get; set; }

		// Resim yükleme için Create Edit sayfalarında
		public IFormFile? ImageFile { get; set; }
	}
}

