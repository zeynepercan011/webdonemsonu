using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webdonemsonu.Models.ViewModels
{
	public class CartVM
	{
		public List<CartItemVM> Items { get; set; } = new();

		[Display(Name = "Toplam Tutar")]
		public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
	}

	public class CartItemVM
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public byte[] ProductImage { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPrice => UnitPrice * Quantity;

		[NotMapped] // Veritabanına kaydedilmeyecek
		public string ProductImageDisplay =>
	   ProductImage != null && ProductImage.Length > 0
		   ? $"data:image/jpeg;base64,{Convert.ToBase64String(ProductImage)}"
		   : "/images/default-product.png";
	}
}