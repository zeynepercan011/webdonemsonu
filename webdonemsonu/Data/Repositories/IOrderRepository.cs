using webdonemsonu.Models;

namespace webdonemsonu.Data.Repositories
{
	public interface IOrderRepository
	{
		Task CreateOrderAsync(Order order);
		Task<List<Order>> GetOrdersByUserIdAsync(string userId);
	}

}
