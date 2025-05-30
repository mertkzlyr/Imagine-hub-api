using ImagineHubAPI.DTOs.AuthDTOs;
using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetUserByIdAsync(int id);
    Task<Result<UserDto>> GetByUsernameAsync(string username);
    Task<Result<LoginResponse>> AuthenticateAsync(LoginRequest request);
    Task<Result> RegisterUser(RegisterDto registerDto);
    Task<Result<UserDto>> UpdateUserAsync(int userId, UpdateUserDto updateDto);
    Task<Result> UpdateProfilePictureAsync(int userId, IFormFile profilePicture);
    Task<Result> UpdatePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<Result> DeleteAccountAsync(int userId, string password);
    Task<Result> SendMagicLinkAsync(string email, string baseUrl);
    Task<Result> ResetPasswordWithTokenAsync(string token, string newPassword);
}