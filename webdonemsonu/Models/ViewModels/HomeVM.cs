using System.Collections.Generic;
using webdonemsonu.Models;

namespace webdonemsonu.Models.ViewModels
{
	public class HomeVM
	{
		public Dictionary<string, List<Product>> CategoriesWithProducts { get; set; }
		public List<Product> FeaturedProducts { get; set; }
	}
}