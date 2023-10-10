using MarketplaceDesktop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class OrderController 
{
	private readonly Marketplace1Context _context;

	public OrderController(Marketplace1Context context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Order>> GetOrders()
	{
		return await _context.Orders.ToListAsync();
	}

	public async Task<Order> GetOrder(int id)
	{
		return await _context.Orders.FindAsync(id);
	}

	public async Task<Order> PostOrder(Order Order)
	{
		_context.Orders.Add(Order);
		await _context.SaveChangesAsync();

		return Order;
	}

	public async Task<Order> PutOrder(int id, Order Order)
	{
		if (id != Order.OrderId)
		{
			return null;
		}

		_context.Entry(Order).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!OrderExists(id))
			{
				return null;
			}
			else
			{
				throw;
			}
		}

		return Order;
	}

	public async Task<Order> DeleteOrder(int id)
	{
		var Order = await _context.Orders.FindAsync(id);
		if (Order == null)
		{
			return null;
		}

		_context.Orders.Remove(Order);
		await _context.SaveChangesAsync();

		return Order;
	}

	private bool OrderExists(int id)
	{
		return _context.Orders.Any(e => e.OrderId == id);
	}
}
