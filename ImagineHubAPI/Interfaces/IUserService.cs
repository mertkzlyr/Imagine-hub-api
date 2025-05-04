using ImagineHubAPI.DTOs.AuthDTOs;
using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(int id);
    Task<LoginResponse?> AuthenticateAsync(LoginRequest request);
    Task<string> RegisterUser(RegisterDto registerDto);
}