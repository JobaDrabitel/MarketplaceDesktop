﻿using MarketplaceDesktop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserController
{
	private readonly Marketplace1Context _context;

	public UserController(Marketplace1Context context)
	{
		_context = context;
	}

	public async Task<List<User>> GetUsers()
	{
		return await _context.Users.ToListAsync();
	}

	public async Task<User> GetUser(int id)
	{
		return await _context.Users.FindAsync(id);
	}

	public async Task<User> PostUser(User user)
	{
		try
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex) { return null; }
		return user;
	}

	public async Task<User> PutUser(int id, User user)
	{
		if (id != user.UserId)
		{
			return null;
		}

		_context.Entry(user).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!UserExists(id))
			{
				return null;
			}
			else
			{
				throw;
			}
		}

		return user;
	}

	public async Task<User> DeleteUser(int id)
	{
		var user = await _context.Users.FindAsync(id);
		if (user == null)
		{
			return null;
		}

		_context.Users.Remove(user);
		await _context.SaveChangesAsync();

		return user;
	}

	private bool UserExists(int id)
	{
		return _context.Users.Any(e => e.UserId == id);
	}
	public async Task<User> AuthorizeUser(string email, string password)
	{
		return await _context.Users.FirstOrDefaultAsync<User>(u => u.Email == email && u.PasswordHash == password);
	}
}
