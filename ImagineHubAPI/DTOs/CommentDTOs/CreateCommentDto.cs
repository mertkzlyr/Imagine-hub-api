namespace ImagineHubAPI.DTOs.CommentDTOs;

public class CreateCommentDto
{
    public Guid PostId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }  // Optional for replies
}