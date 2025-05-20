using System.ComponentModel.DataAnnotations;

namespace ImagineHubAPI.DTOs.UserDTOs;

public class UpdatePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; }
    
    [Required] public string NewPassword { get; set; }
}