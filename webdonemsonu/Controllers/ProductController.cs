using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webdonemsonu.Data.Repositories;
using webdonemsonu.Models.ViewModels;
using webdonemsonu.Services;

namespace webdonemsonu.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductRepository _productRepo;
		private readonly IImageService _imageService;
		private readonly ICartService _cartService;
		private readonly ILogger<ProductController> _logger;

		public ProductController(
			IProductRepository productRepo,
			IImageService imageService,
			ICartService cartService,
			ILogger<ProductController> logger)
		{
			_productRepo = productRepo;
			_imageService = imageService;
			_cartService = cartService;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			try
			{
				var product = await _productRepo.GetProductByIdAsync(id);
				if (product == null)
				{
					_logger.LogWarning($"Product with id {id} not found");
					return NotFound();
				}

				var model = new ProductVM
				{
					Id = product.Id,
					Name = product.Name,
					Description = product.Description,
					Price = product.Price,
					Image = product.Image,
					CategoryId = product.CategoryId,
					CategoryName = product.Category?.Name
				};

				return View(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error retrieving product details for id {id}");
				return RedirectToAction("Error", "Home");
			}
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
		{
			try
			{
				if (quantity < 1)
				{
					TempData["Error"] = "Geçersiz miktar!";
					return RedirectToAction("Details", new { id = productId });
				}

				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
				{
					_logger.LogWarning("Unauthorized cart access attempt");
					return Challenge();
				}

				await _cartService.AddToCartAsync(userId, productId, quantity);

				TempData["Success"] = "Ürün sepete eklendi!";
				return RedirectToAction("Details", new { id = productId });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error adding product {productId} to cart");
				TempData["Error"] = "Sepete ekleme işlemi başarısız!";
				return RedirectToAction("Details", new { id = productId });
			}
		}
	}
}