using ImagineHubAPI.Data;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Repositories;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<User> GetByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        return user;
    }

    public async Task<User> AddAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task<User> UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task<User> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}