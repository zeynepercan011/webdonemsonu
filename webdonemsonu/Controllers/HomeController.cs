using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webdonemsonu.Data;
using webdonemsonu.Data.Repositories;
using webdonemsonu.Models;
using webdonemsonu.Models.ViewModels;

namespace webdonemsonu.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IProductRepository _productRepo;

		public HomeController(ApplicationDbContext context, IProductRepository productRepo)
		{
			_context = context;
			_productRepo = productRepo;
		}

		public async Task<IActionResult> Index()
		{
			// Ürünleri kategorisiyle birlikte çek
			var products = await _context.Products
				.Include(p => p.Category)
				.Where(p => p.Category != null) // Güvenlik: Null kategori varsa dýþla
				.ToListAsync();

			// Kategorilere göre grupla
			var productsByCategory = products
				.GroupBy(p => p.Category.Name)
				.ToDictionary(g => g.Key, g => g.ToList());

			var model = new HomeVM
			{
				CategoriesWithProducts = productsByCategory,

				FeaturedProducts = products
					.OrderByDescending(p => p.Id)
					.Take(4)
					.ToList()
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Search(string query)
		{
			if (string.IsNullOrWhiteSpace(query))
				return RedirectToAction("Index");

			var results = await _context.Products
				.Include(p => p.Category)
				.Where(p =>
					(p.Name.Contains(query) || p.Description.Contains(query))
					&& p.Category != null) // Yine null kategori kontrolü
				.ToListAsync();

			return View("Index", new HomeVM
			{
				FeaturedProducts = results,
				CategoriesWithProducts = results
					.GroupBy(p => p.Category.Name)
					.ToDictionary(g => g.Key, g => g.ToList())
			});
		}
	}
}
