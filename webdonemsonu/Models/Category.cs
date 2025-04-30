using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace webdonemsonu.Models
{
	public class Category //Ürünün hangi kategoriye ait olduğunu belirler.
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public virtual ICollection<Product> Products { get; set; }
		//bir kategoride birden fazla ürün var.
	}
}
