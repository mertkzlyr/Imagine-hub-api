using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user) => new UserDto
    {
        Id = user.Id,
        Username = user.Username,
        Name = user.Name,
        Surname = user.Surname,
        City = user.City,
        Country = user.Country,
        CreatedAt = user.CreatedAt,
        ProfilePicture = user.ProfilePicture
    };
}