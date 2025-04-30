using Microsoft.EntityFrameworkCore;
using webdonemsonu.Models;

namespace webdonemsonu.Data.Repositories
{
	public class OrderRepository : IOrderRepository
		//Sipariş ile ilgili veri tabanı işlemlerini yönetir.
	{
		private readonly ApplicationDbContext _context;

		public OrderRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task CreateOrderAsync(Order order)
		{
			//Yeni siparişi veri tabanına ekledik.
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
		{
			//Belirli bir kullancıya ait tüm ürünler getirildi.
			return await _context.Orders
				.Where(o => o.UserId == userId)
				.Include(o => o.OrderItems)
				.OrderByDescending(o => o.OrderDate)
				.ToListAsync();
		}

	}

}
