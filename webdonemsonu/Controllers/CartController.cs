using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webdonemsonu.Services;
using webdonemsonu.Models.ViewModels;
using webdonemsonu.Data.Repositories;
using System.Security.Claims;
using webdonemsonu.Models;
using webdonemsonu.Models;

namespace webdonemsonu.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private readonly ICartService _cartService;
		private readonly IProductRepository _productRepo;
		private readonly IOrderRepository _orderRepository;

		public CartController(ICartService cartService, IProductRepository productRepo, IOrderRepository orderRepository)
		{
			_cartService = cartService;
			_productRepo = productRepo;
			_orderRepository = orderRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var cart = await _cartService.GetCartAsync(userId);
			return View(cart); //cart.cshtml'e model gönderilir.
		}

		[HttpPost]
		public async Task<IActionResult> RemoveItem(int productId)
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			await _cartService.RemoveFromCartAsync(userId, productId);

			TempData["Success"] = "Ürün sepetten kaldırıldı!";
			return RedirectToAction("Index");
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Checkout() //Ödeme kısmı
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var cart = await _cartService.GetCartAsync(userId);

			if (cart == null || !cart.Items.Any())
			{
				TempData["Error"] = "Sepetiniz boş!";
				return RedirectToAction("Index");
			}

			var order = new Order
			{
				UserId = userId,
				OrderDate = DateTime.UtcNow,
				TotalPrice = cart.TotalPrice,
				Status = "Tamamlandı",
				ShippingAddress = "Adres Girilmedi",
				OrderNote = "Sipariş notu yok",
				OrderItems = cart.Items.Select(item => new OrderItem
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					UnitPrice = item.UnitPrice
				}).ToList()
			};

			await _orderRepository.CreateOrderAsync(order);
			await _cartService.ClearCartAsync(userId);

			TempData["OrderNumber"] = order.Id.ToString().PadLeft(8, '0');
			return RedirectToAction("Success");
		}


		[HttpGet]
		public IActionResult Success()
		{
			return View();
		}

	}
}