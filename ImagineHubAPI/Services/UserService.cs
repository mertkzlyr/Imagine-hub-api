using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ImagineHubAPI.Config;
using ImagineHubAPI.DTOs.AuthDTOs;
using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Helpers;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ImagineHubAPI.Services;

public class UserService(IUserRepository userRepository, ITokenService tokenService, PasswordHasherService hasher) : IUserService
{
    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);

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
            Followers = user.Followers.Select(f => new FollowerDto
            {
                Id = f.Follower.Id,
                Username = f.Follower.Username
            }).ToList(),
            Following = user.Following.Select(f => new FollowingDto
            {
                Id = f.Followee.Id,
                Username = f.Followee.Username
            }).ToList()
        };
        
        return userDto;
    }

    public async Task<LoginResponse?> AuthenticateAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            return null;

        var passwordIsValid = hasher.VerifyPassword(request.Password, user.Password);
        if (!passwordIsValid)
            return null;

        var token = tokenService.CreateToken(user);
        return new LoginResponse { Token = token };
    }

    public async Task<string> RegisterUser(RegisterDto registerDto)
    {
        if (await userRepository.GetByEmailAsync(registerDto.Email) != null)
            return "Email is already in use.";

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
        return "User registered successfully.";
    }
}