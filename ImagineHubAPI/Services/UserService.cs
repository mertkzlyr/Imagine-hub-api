using ImagineHubAPI.DTOs.AuthDTOs;
using ImagineHubAPI.DTOs.PostDTOs;
using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Helpers;
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
            Following = user.Following.Count,
            PostCount = user.Posts.Count,
            Posts = user.Posts.Select(p => new PostDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Username = p.User.Username,
                Name = p.User.Name,
                Surname = p.User.Surname,
                LikeCount = p.Likes.Count,
                CommentCount = p.Comments.Count,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                CreatedAt = p.CreatedAt
            }).ToList()
        };

        return new Result<UserDto>
        {
            Success = true,
            Message = "User retrieved successfully.",
            Data = userDto
        };
    }

    public async Task<Result<UserDto>> GetByUsernameAsync(string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        
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
            Following = user.Following.Count,
            PostCount = user.Posts.Count,
            Posts = user.Posts.Select(p => new PostDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Username = p.User.Username,
                Name = p.User.Name,
                Surname = p.User.Surname,
                LikeCount = p.Likes.Count,
                CommentCount = p.Comments.Count,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                CreatedAt = p.CreatedAt
            }).ToList()
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

        if (await userRepository.GetByUsernameAsync(registerDto.Username) != null)
        {
            return new Result
            {
                Success = false,
                Message = "Username is already in use."
            };
        }

        var hashedPassword = hasher.HashPassword(registerDto.Password);

        var profilePicFileName = await PictureSaver.SaveProfilePictureAsync(registerDto.ProfilePicture, registerDto.Username);

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
            ProfilePicture = profilePicFileName,
            GenerationToken = 10
        };

        await userRepository.AddAsync(user);

        return new Result
        {
            Success = true,
            Message = "User registered successfully."
        };
    }

    public async Task<Result<UserDto>> UpdateUserAsync(int userId, UpdateUserDto updateDto)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return new Result<UserDto> { Success = false, Message = "User not found." };

        // Update allowed fields
        user.Username = updateDto.Username ?? user.Username;
        user.Name = updateDto.Name ?? user.Name;
        user.Surname = updateDto.Surname ?? user.Surname;
        user.MiddleName = updateDto.MiddleName ?? user.MiddleName;
        user.City = updateDto.City ?? user.City;
        user.State = updateDto.State ?? user.State;
        user.Country = updateDto.Country ?? user.Country;
        user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await userRepository.UpdateAsync(user);

        var userDto = new UserDto
        {
            Id = updatedUser.Id,
            Username = updatedUser.Username,
            Name = updatedUser.Name,
            Surname = updatedUser.Surname,
            City = updatedUser.City,
            Country = updatedUser.Country,
            CreatedAt = updatedUser.CreatedAt,
            ProfilePicture = updatedUser.ProfilePicture,
            Followers = updatedUser.Followers.Count,
            Following = updatedUser.Following.Count,
            PostCount = updatedUser.Posts.Count,
            Posts = user.Posts.Select(p => new PostDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Username = p.User.Username,
                Name = p.User.Name,
                Surname = p.User.Surname,
                LikeCount = p.Likes.Count,
                CommentCount = p.Comments.Count,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                CreatedAt = p.CreatedAt
            }).ToList()
        };

        return new Result<UserDto> { Success = true, Data = userDto, Message = "User updated successfully." };
    }

    public async Task<Result> UpdateProfilePictureAsync(int userId, IFormFile profilePicture)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return new Result { Success = false, Message = "User not found." };

        var newFileName = await PictureSaver.SaveProfilePictureAsync(profilePicture, user.Username);
        user.ProfilePicture = newFileName;
        user.UpdatedAt = DateTime.UtcNow;

        await userRepository.UpdateAsync(user);

        return new Result { Success = true, Message = "Profile picture updated." };
    }
    
    public async Task<Result> UpdatePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return new Result { Success = false, Message = "User not found." };

        if (!hasher.VerifyPassword(currentPassword, user.Password))
            return new Result { Success = false, Message = "Current password is incorrect." };

        user.Password = hasher.HashPassword(newPassword);

        try
        {
            await userRepository.UpdateAsync(user);
            return new Result { Success = true, Message = "Password updated successfully." };
        }
        catch (Exception ex)
        {
            return new Result { Success = false, Message = $"Failed to update password: {ex.Message}" };
        }
    }

    public async Task<Result> DeleteAccountAsync(int userId, string password)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return new Result { Success = false, Message = "User not found." };

        if (!hasher.VerifyPassword(password, user.Password))
            return new Result { Success = false, Message = "Incorrect password." };

        try
        {
            await userRepository.DeleteAsync(userId);
            return new Result { Success = true, Message = "Account deleted successfully." };
        }
        catch (Exception ex)
        {
            return new Result { Success = false, Message = $"Error deleting account: {ex.Message}" };
        }
    }
}