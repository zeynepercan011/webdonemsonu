using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webdonemsonu.Models;

namespace webdonemsonu.Data.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _context;

		public ProductRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Product>> GetAllProductsAsync()
		{
			//Tüm ürünleri kategorileri ile birlikte getirir.
			return await _context.Products.Include(p => p.Category).ToListAsync();
		}

		public async Task<Product?> GetProductByIdAsync(int id)
		{
			//Belirli bir ürünü id’ye göre getirir (kategori bilgisi dahil).
			return await _context.Products
				.Include(p => p.Category)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
		{
			//Belirli bir kategoriye ait tüm ürünleri getirir.
			return await _context.Products
				.Where(p => p.CategoryId == categoryId)
				.ToListAsync();
		}
	}
	//Repositoryde veri tabanı ile direkt bağlantı kurulur. Sadece CRUD işlemleri yapılır.
	//Service genelde repositoryi kullanarak CRUD dışı işlemler yapar.
}
