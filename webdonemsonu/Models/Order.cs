using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webdonemsonu.Models
{
	public class Order //Siparişin bilgileri.
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(450)]
		public string UserId { get; set; } //Siparişi veren kullanıcı.

		[ForeignKey("UserId")] //Users tableındaki UserId'den bunu belirleriz.
		//Bundan dolayı foreign key.
		public virtual ApplicationUser User { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		[Display(Name = "Sipariş Tarihi")]
		public DateTime OrderDate { get; set; } = DateTime.UtcNow;

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		[Display(Name = "Toplam Tutar")]
		public decimal TotalPrice { get; set; }

		[Required]
		[StringLength(50)]
		[Display(Name = "Durum")]
		public string Status { get; set; } = "Hazırlanıyor";

		[Display(Name = "Teslimat Adresi")]
		[StringLength(500)]
		public string ShippingAddress { get; set; }

		[Display(Name = "Sipariş Notu")]
		[StringLength(1000)]
		public string OrderNote { get; set; }

		//Siparişe ait ürünleri temsil eder. Bir siparişte birden fazla ürün olabilir.
		//Lazy loading için virtual kullanıldı. İhtiyaç duyana kadar yükleme dursun diye yani.
		public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
	}

	public class OrderItem //Sipariş içindeki her bir ürünü temsil eder.
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int OrderId { get; set; } //Hangi siparişe ait bir ürün olduğunu belirtir.

		[ForeignKey("OrderId")]
		public virtual Order Order { get; set; }

		[Required]
		public int ProductId { get; set; } //Sipariş edilen ürün hangi ürün?

		[ForeignKey("ProductId")]
		public virtual Product Product { get; set; }

		[Required]
		[Range(1, 1000)]
		[Display(Name = "Adet")]
		public int Quantity { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		[Display(Name = "Birim Fiyat")]
		public decimal UnitPrice { get; set; }

		[NotMapped]
		[Display(Name = "Toplam")]
		public decimal TotalPrice => UnitPrice * Quantity;
	}
}