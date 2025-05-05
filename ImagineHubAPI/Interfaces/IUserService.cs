using ImagineHubAPI.DTOs.AuthDTOs;
using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetUserByIdAsync(int id);
    Task<Result<LoginResponse>> AuthenticateAsync(LoginRequest request);
    Task<Result> RegisterUser(RegisterDto registerDto);
}