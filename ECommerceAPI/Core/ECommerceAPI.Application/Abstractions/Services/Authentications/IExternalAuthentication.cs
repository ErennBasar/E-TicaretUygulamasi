namespace ECommerceAPI.Application.Abstractions.Services.Authentications;

public interface IExternalAuthentication
{
    Task<DTOs.Token> FacebookLoginAsync(string authToken, int accessTokenLifeTimeSeconds);
    Task<DTOs.Token> GoogleLoginAsync(string idToken, int accessTokenLifeTimeSeconds);
}