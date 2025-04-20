using ImagineHubAPI.DTOs.AuthDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int id);
    Task<LoginResponse?> AuthenticateAsync(LoginRequest request);
    Task<string> RegisterUser(RegisterDto registerDto);
}