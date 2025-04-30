using Microsoft.AspNetCore.Mvc;

namespace webdonemsonu.Models.ViewModels
{
	public class AdminPanelVM
	{
		public List<Category> Categories { get; set; }
		public List<Product> Products { get; set; }
		public Product NewProduct { get; set; } = new Product();

	}

}
