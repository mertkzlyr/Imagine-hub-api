using ImagineHubAPI.DTOs.AuthDTOs;
using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Services;

public class UserService(IUserRepository userRepository, ITokenService tokenService, PasswordHasherService hasher) : IUserService
{
    public async Task<Result<UserDto>> GetUserByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return new Result<UserDto>
            {
                Success = false,
                Message = "User not found.",
                Data = null
            };
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Surname = user.Surname,
            City = user.City,
            Country = user.Country,
            CreatedAt = user.CreatedAt,
            ProfilePicture = user.ProfilePicture,
            Followers = user.Followers.Count,
            Following = user.Following.Count
        };

        return new Result<UserDto>
        {
            Success = true,
            Message = "User retrieved successfully.",
            Data = userDto
        };
    }


    public async Task<Result<LoginResponse>> AuthenticateAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null || !hasher.VerifyPassword(request.Password, user.Password))
        {
            return new Result<LoginResponse>
            {
                Success = false,
                Message = "Invalid email or password.",
                Data = null
            };
        }

        var token = tokenService.CreateToken(user);
        return new Result<LoginResponse>
        {
            Success = true,
            Message = "Authentication successful.",
            Data = new LoginResponse { Token = token }
        };
    }

    public async Task<Result> RegisterUser(RegisterDto registerDto)
    {
        if (await userRepository.GetByEmailAsync(registerDto.Email) != null)
        {
            return new Result
            {
                Success = false,
                Message = "Email is already in use."
            };
        }

        var hashedPassword = hasher.HashPassword(registerDto.Password);

        var user = new User
        {
            Username = registerDto.Username,
            Name = registerDto.Name,
            Surname = registerDto.Surname,
            MiddleName = registerDto.MiddleName,
            Email = registerDto.Email,
            Password = hashedPassword,
            PhoneNumber = registerDto.PhoneNumber,
            City = registerDto.City,
            State = registerDto.State,
            Country = registerDto.Country,
            CreatedAt = DateTime.UtcNow,
        };

        await userRepository.AddAsync(user);

        return new Result
        {
            Success = true,
            Message = "User registered successfully."
        };
    }

}