using ImagineHubAPI.DTOs.AuthDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IAdminService
{
    Task<List<User>> GetAllUsersAsync();
    Task<LoginResponse?> AdminLoginAsync(LoginRequest request);
}