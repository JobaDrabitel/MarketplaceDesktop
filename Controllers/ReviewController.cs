﻿using MarketplaceDesktop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReviewController 
{
	private readonly Marketplace1Context _context;

	public ReviewController(Marketplace1Context context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Review>> GetReviews()
	{
		return await _context.Reviews.ToListAsync();
	}

	public async Task<Review> GetReview(int id)
	{
		return await _context.Reviews.FindAsync(id);
	}

	public async Task<Review> PostReview(Review Review)
	{
		_context.Reviews.Add(Review);
		await _context.SaveChangesAsync();

		return Review;
	}

	public async Task<Review> PutReview(int id, Review Review)
	{
		if (id != Review.UserId)
		{
			return null;
		}

		_context.Entry(Review).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!ReviewExists(id))
			{
				return null;
			}
			else
			{
				throw;
			}
		}

		return Review;
	}

	public async Task<Review> DeleteReview(int id)
	{
		var Review = await _context.Reviews.FindAsync(id);
		if (Review == null)
		{
			return null;
		}

		_context.Reviews.Remove(Review);
		await _context.SaveChangesAsync();

		return Review;
	}

	private bool ReviewExists(int id)
	{
		return _context.Reviews.Any(e => e.UserId == id);
	}
}
