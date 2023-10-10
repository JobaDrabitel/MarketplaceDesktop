using MarketplaceDesktop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class ProductController 
{
	private readonly Marketplace1Context _context;

	public ProductController(Marketplace1Context context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Product>> GetProducts()
	{
		return await _context.Products.ToListAsync();
	}

	public async Task<Product> GetProduct(int id)
	{
		return await _context.Products.FindAsync(id);
	}

	public async Task<Product> PostProduct(Product Product)
	{
		_context.Products.Add(Product);
		await _context.SaveChangesAsync();

		return Product;
	}

	public async Task<Product> PutProduct(int id, Product Product)
	{
		if (id != Product.ProductId)
		{
			return null;
		}

		_context.Entry(Product).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!ProductExists(id))
			{
				return null;
			}
			else
			{
				throw;
			}
		}

		return Product;
	}

	public async Task<Product> DeleteProduct(int id)
	{
		var Product = await _context.Products.FindAsync(id);
		if (Product == null)
		{
			return null;
		}

		_context.Products.Remove(Product);
		await _context.SaveChangesAsync();

		return Product;
	}

	private bool ProductExists(int id)
	{
		return _context.Products.Any(e => e.ProductId == id);
	}
}
