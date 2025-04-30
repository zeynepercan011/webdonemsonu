using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webdonemsonu.Models
{
	public class CartItem //Bunun kullanıldığı yer sepet sayfasıdır.
		//Order classından farkı ürün sepete ekleyince ortaya çıkmasdıdır.
		//Order sipariş verildikten sonraki adımdır.
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string UserId {  get; set; } //Ürünler hangi kullanıcıya ait.
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }
		[Required]
		public int ProductId { get; set; } //Hangi ürün sepette.
		[ForeignKey("ProductId")]
		public virtual Product Product { get; set; }
		[Required]
		public int Quantity { get; set; }
	}
}
