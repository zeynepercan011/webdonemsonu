using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webdonemsonu.Models
{
	public class Product //Ürünü temsil eden class.
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		[Column(TypeName = "decimal(18,2)")] //18 rakam 2 ondalık.
		public decimal Price { get; set; }
		public byte[] Image { get; set; }
		[Required]
		public int CategoryId { get; set; } //Ürünün hangi kategoriye ait olduğunu bilmemiz lazım.
		[ForeignKey("CategoryId")]
		public virtual Category Category { get; set; }
		//kategori hep çekilmesin gerektiği zaman çekilsin diye virtual kullanıldı.
	}
}
