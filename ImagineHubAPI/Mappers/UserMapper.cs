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
        MiddleName = user.MiddleName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        City = user.City,
        State = user.State,
        Country = user.Country,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt,
        ProfilePicture = user.ProfilePicture
    };
}