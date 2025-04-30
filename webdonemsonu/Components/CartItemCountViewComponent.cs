using Microsoft.AspNetCore.Mvc;
using webdonemsonu.Services;

namespace webdonemsonu.Components
{
	public class CartItemCountViewComponent : ViewComponent
		//Navbarda sepet simgesinde kaç ürün var göstermek için.
	{
		private readonly ICartService _cartService;

		public CartItemCountViewComponent(ICartService cartService)
		{
			_cartService = cartService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var userId = UserClaimsPrincipal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var count = userId != null ? await _cartService.GetCartItemCountAsync(userId) : 0;
			return Content(count.ToString());
		}
	}
}