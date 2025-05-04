using ImagineHubAPI.Data;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagineHubAPI.Repositories;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<User> GetByIdAsync(int id)
    {
        var user = await context.Users
            .Include(u => u.Followers)
            .ThenInclude(uf => uf.Follower)
            .Include(u => u.Following)
            .ThenInclude(uf => uf.Followee)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        return user;
    }

    public async Task<User> AddAsync(User entity)
    {
        await context.Users.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<User> UpdateAsync(User entity)
    {
        var existingUser = await context.Users.FindAsync(entity.Id);
        if (existingUser == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        context.Entry(existingUser).CurrentValues.SetValues(entity);
        await context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<User> DeleteAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        return user;
    }
}