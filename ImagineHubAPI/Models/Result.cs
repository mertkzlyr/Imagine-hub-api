namespace ImagineHubAPI.Models;

public class Result
{
    public bool Success { get; set; }
    public string Message { get; set; }
}

public class Result<T> : Result
{
    public T Data { get; set; }
}

public class ResultList<T> : Result
{
    public List<T> Data { get; set; }
    public PaginationInformation Pagination { get; set; }
}