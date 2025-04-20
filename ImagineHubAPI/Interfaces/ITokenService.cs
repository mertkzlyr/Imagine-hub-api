namespace ImagineHubAPI.Interfaces;

public interface ITokenService
{
    string CreateToken<T>(T entity);
}