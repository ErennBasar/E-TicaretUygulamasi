namespace ECommerceAPI.Application.Abstractions.Services.Authentications;

public interface IInternalAuthentication
{
    Task<DTOs.Token> LoginAsync(String usernameOrEmail, String password, int accessTokenLifeTimeSeconds);
}