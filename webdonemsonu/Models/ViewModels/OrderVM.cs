using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webdonemsonu.Models.ViewModels
{
	public class OrderVM
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Adres zorunludur")]
		[Display(Name = "Teslimat Adresi")]
		public string ShippingAddress { get; set; }

		[Display(Name = "Sipariş Tarihi")]
		public DateTime OrderDate { get; set; } = DateTime.Now;

		[Display(Name = "Toplam Tutar")]
		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalPrice { get; set; }

		[Display(Name = "Sipariş Durumu")]
		public string Status { get; set; } = "Hazırlanıyor";

		[Display(Name = "Kart Sahibi Adı")]
		public string? CardHolderName { get; set; }

		[Display(Name = "Kart Numarası")]
		[CreditCard(ErrorMessage = "Geçersiz kart numarası")]
		public string? CardNumber { get; set; }

		[Display(Name = "Son Kullanma (AA/YY)")]
		[RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Geçersiz tarih formatı")]
		public string? CardExpiry { get; set; }

		[Display(Name = "CVV")]
		[StringLength(3, MinimumLength = 3, ErrorMessage = "CVV 3 haneli olmalıdır")]
		public string? CardCvv { get; set; }

		// İlişkiler
		public List<OrderItemVM> OrderItems { get; set; } = new();
	}

	public class OrderItemVM
	{
		public int ProductId { get; set; }

		[Display(Name = "Ürün Adı")]
		public string ProductName { get; set; }

		[Display(Name = "Adet")]
		[Range(1, 100, ErrorMessage = "1-100 arası değer girin")]
		public int Quantity { get; set; }

		[Display(Name = "Birim Fiyat")]
		[Column(TypeName = "decimal(18,2)")]
		public decimal UnitPrice { get; set; }

		[Display(Name = "Toplam")]
		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalPrice => UnitPrice * Quantity;
	}
}