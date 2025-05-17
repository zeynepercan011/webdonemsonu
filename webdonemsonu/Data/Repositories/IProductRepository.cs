using Microsoft.AspNetCore.Mvc;
using webdonemsonu.Models;

namespace webdonemsonu.Data.Repositories
{
	public interface IProductRepository
	{
		Task<List<Product>> GetAllProductsAsync();
		Task<Product?> GetProductByIdAsync(int id);
		Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
		Task<List<Category>> GetAllCategoriesAsync();
	}
}
