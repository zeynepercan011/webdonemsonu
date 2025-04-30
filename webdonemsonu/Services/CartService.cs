using Microsoft.EntityFrameworkCore;
using webdonemsonu.Data;
using webdonemsonu.Models;
using webdonemsonu.Models.ViewModels;

namespace webdonemsonu.Services
{
	public class CartService : ICartService //Alışveriş sepeti ile ilgili işlemler.
	{
		private readonly ApplicationDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task AddToCartAsync(string userId, int productId, int quantity = 1)
		{
			var existingItem = await _context.CartItems
				.AsNoTracking() // Performans için
				.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

			if (existingItem != null) //sepette aynı üründen zaten varsa sayısını arttırmak için.
			{
				existingItem.Quantity += quantity;
				_context.Update(existingItem);
			}
			else //Ürün sepette yoksa
			{
				_context.CartItems.Add(new CartItem
				{
					UserId = userId, //Ürün için yeni kayıt oluşturduk.
					ProductId = productId,
					Quantity = quantity
				});
			}
			await _context.SaveChangesAsync(); //Veri tabanına kaydettik.
		}

		public async Task<CartVM> GetCartAsync(string userId) //Kullanıcının sepetini getirir.
			//O yüzden Kart view modelini kullandık.
		{
			var cartItems = await _context.CartItems
				.Where(ci => ci.UserId == userId)
				.Include(ci => ci.Product)
				.ToListAsync();

			var cartVM = new CartVM //şimdi bunu CartVM formatına dönüştürüyoruz.
			{
				Items = cartItems.Select(ci => new CartItemVM
				{
					ProductId = ci.ProductId,
					ProductName = ci.Product.Name,
					ProductImage = ci.Product.Image, // Doğrudan byte[] atıyoruz
					Quantity = ci.Quantity,
					UnitPrice = ci.Product.Price
				}).ToList()
			};

			return cartVM;
		}


		public async Task RemoveFromCartAsync(string userId, int productId)
		{
			//Sepetten kullanıcı ve ürün id'sine bakarak belirli bir ürünü kaldırır.
			var item = await _context.CartItems
				.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

			if (item != null)
			{
				_context.CartItems.Remove(item);
				await _context.SaveChangesAsync();
			}
		}

		public async Task ClearCartAsync(string userId)
		{
			var items = await _context.CartItems
				.Where(ci => ci.UserId == userId)
				.ToListAsync();

			_context.CartItems.RemoveRange(items); //Sepetteki her ürünü siler.
			await _context.SaveChangesAsync();
		}

		public async Task<int> GetCartItemCountAsync(string userId)
		{
			return await _context.CartItems
				.Where(ci => ci.UserId == userId)
				.SumAsync(ci => ci.Quantity); //Sepetteki ürün adedini verir.
		}
	}
}