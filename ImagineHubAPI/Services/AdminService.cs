using ImagineHubAPI.DTOs.AuthDTOs;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Services;

public class AdminService(IAdminRepository adminRepository, IUserRepository userRepository, ITokenService tokenService, PasswordHasherService hasher) : IAdminService
{
    public async Task<LoginResponse?> AdminLoginAsync(LoginRequest request)
    {
        var admin = await adminRepository.GetByUsernameAsync(request.Email);

        var decryptedPassword = hasher.VerifyPassword(request.Password, admin.Password);
        
        var passwordIsValid = hasher.VerifyPassword(request.Password, admin.Password);
        if (!passwordIsValid)
            return null;

        var token = tokenService.CreateToken(admin);
        return new LoginResponse { Token = token };
    }
    
    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = await adminRepository.GetAllUsersAsync();
        if (users == null)
        {
            throw new KeyNotFoundException("No users found.");
        }
        
        return users;
    }
    
    public async Task<string> DeleteUserAsync(int id)
    {
        await userRepository.DeleteAsync(id);
        return $"User with ID {id} has been successfully deleted.";
    }
}