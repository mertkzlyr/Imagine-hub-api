using ImagineHubAPI.Data;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagineHubAPI.Repositories;

public class AdminRepository(DataContext context) : IAdminRepository
{
    public async Task<List<User>> GetAllUsersAsync()
    {
        // get all users
        var users = await context.Users.ToListAsync();
        if (users == null)
        {
            throw new KeyNotFoundException("No users found.");
        }
        
        return users;
    }

    public async Task<Admin> GetByUsernameAsync(string username)
    {
        // get admin by username
        var admin = await context.Admins.FirstOrDefaultAsync(a => a.Username == username);
        if (admin == null)
        {
            throw new KeyNotFoundException("Admin not found.");
        }
        
        return admin;
    }

    public async Task<Admin> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Admin> AddAsync(Admin entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Admin> UpdateAsync(Admin entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Admin> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}