using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webdonemsonu.Data;
using webdonemsonu.Models;
using webdonemsonu.Models.ViewModels;
using webdonemsonu.Services;

namespace webdonemsonu.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IImageService _imageService;

		public AdminController(ApplicationDbContext context, IImageService imageService)
		{
			_context = context;
			_imageService = imageService;
		}

		// /Admin
		[HttpGet]
		public IActionResult Index()
		{
			return RedirectToAction("AdminPanel");
		}

		// Admin Paneli Ana Sayfası
		[HttpGet]
		public async Task<IActionResult> AdminPanel()
		{
			var products = await _context.Products.Include(p => p.Category).ToListAsync();
			var categories = await _context.Categories.ToListAsync();

			var model = new AdminPanelVM
			{
				Products = products,
				Categories = categories
			};

			return View(model); // Views/Admin/AdminPanel.cshtml
		}

		// Yeni ürün ekleme
		[HttpPost]
		public async Task<IActionResult> AddProduct(AdminPanelVM model, IFormFile ImageFile) // <- NewProduct yerine AdminPanelVM al
		{
			if (string.IsNullOrWhiteSpace(model.NewProduct.Name) ||
				model.NewProduct.CategoryId == 0 ||
				model.NewProduct.Price <= 0)
			{
				TempData["Error"] = "Tüm alanları doldurun.";
				return RedirectToAction("AdminPanel");
			}

			var product = new Product
			{
				Name = model.NewProduct.Name,
				Description = model.NewProduct.Description,
				Price = model.NewProduct.Price,
				CategoryId = model.NewProduct.CategoryId,
				Category = await _context.Categories.FindAsync(model.NewProduct.CategoryId)
			};

			if (ImageFile != null && ImageFile.Length > 0)
			{
				product.Image = await _imageService.ConvertImageToByteArrayAsync(ImageFile);
			}

			_context.Products.Add(product);
			await _context.SaveChangesAsync();
			TempData["Success"] = "Ürün başarıyla eklendi.";
			return RedirectToAction("AdminPanel"); // Index yerine AdminPanel'e dön
		}

		// Ürün Silme
		[HttpGet]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				TempData["Error"] = "Ürün bulunamadı.";
				return RedirectToAction("AdminPanel");
			}

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			TempData["Success"] = "Ürün silindi.";
			return RedirectToAction("AdminPanel");
		}

		// Ürün Güncelleme Sayfası
		[HttpGet]
		public async Task<IActionResult> EditProduct(int id)
		{
			var product = await _context.Products.FindAsync(id);
			var categories = await _context.Categories.ToListAsync();

			if (product == null)
			{
				return NotFound();
			}

			ViewBag.Categories = categories;
			return View(product); // Views/Admin/EditProduct.cshtml
		}

		// Ürün Güncelleme Kaydet
		[HttpPost]
		public async Task<IActionResult> EditProduct(Product updatedProduct, IFormFile? ImageFile)
		{
			var product = await _context.Products.FindAsync(updatedProduct.Id);
			if (product == null)
				return NotFound();

			product.Name = updatedProduct.Name;
			product.Description = updatedProduct.Description;
			product.Price = updatedProduct.Price;
			product.CategoryId = updatedProduct.CategoryId;

			if (ImageFile != null && ImageFile.Length > 0)
			{
				product.Image = await _imageService.ConvertImageToByteArrayAsync(ImageFile);
			}

			await _context.SaveChangesAsync();
			TempData["Success"] = "Ürün güncellendi.";
			return RedirectToAction("AdminPanel");
		}
	}
}
