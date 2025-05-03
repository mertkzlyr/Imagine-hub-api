namespace ImagineHubAPI.Extensions;

public static class HttpContextUserExtensions
{
    public static int? GetUserId(this HttpContext context)
    {
        return context.Items["UserId"] as int?;
    }
}