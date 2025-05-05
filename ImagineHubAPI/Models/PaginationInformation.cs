namespace ImagineHubAPI.Models;

public class PaginationInformation
{
    public int From { get; set; }
    public int To { get; set; }
    public int Total { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}