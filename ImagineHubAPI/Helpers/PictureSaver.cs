namespace ImagineHubAPI.Helpers;

public static class PictureSaver
{
    public static async Task<string?> SaveProfilePictureAsync(IFormFile profilePic, string username)
    {
        if (profilePic == null) return null;

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile_pics");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var profilePicFileName = $"profilePic_{username}.webp";
        var filePath = Path.Combine(uploadsFolder, profilePicFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await profilePic.CopyToAsync(stream);
        }
        
        return profilePicFileName;
    }

    public static async Task<string> SavePostPicture(IFormFile picture, Guid postId)
    {
        if (picture == null) return null;
        
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "post_pics");
        
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        
        var postPicName = $"{postId}.webp";
        var filePath = Path.Combine(uploadsFolder, postPicName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await picture.CopyToAsync(stream);
        }
        
        return postPicName;
    }
}