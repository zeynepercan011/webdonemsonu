using Microsoft.AspNetCore.Mvc;
using webdonemsonu.Models.ViewModels;

namespace webdonemsonu.Services
{
	public interface ICartService
	{
		Task AddToCartAsync(string userId, int productId, int quantity = 1);
		Task<CartVM> GetCartAsync(string userId);
		Task RemoveFromCartAsync(string userId, int productId);
		Task ClearCartAsync(string userId);
		Task<int> GetCartItemCountAsync(string userId);
	}
}
