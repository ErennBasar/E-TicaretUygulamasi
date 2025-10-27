using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.Facebook;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ECommerceAPI.Application.Features.Commands.AppUser.FacebookLogin;

public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest,FacebookLoginCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    readonly ITokenHandler  _tokenHandler;
    readonly HttpClient _httpClient;
    readonly IConfiguration _configuration;

    public FacebookLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, 
        ITokenHandler tokenHandler, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request,
        CancellationToken cancellationToken)
    {
        string accessTokenResponse =
            await _httpClient.GetStringAsync
                ($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["Facebook:ClientId"]}&client_secret={_configuration["Facebook:ClientSecret"]}&grant_type=client_credentials");

        FacebookAccessTokenResponseDto facebookAccessTokenResponseDto =
            JsonSerializer.Deserialize<FacebookAccessTokenResponseDto>(accessTokenResponse);

        // Hata kontrolü: Token alınamadıysa
        if (facebookAccessTokenResponseDto == null || string.IsNullOrEmpty(facebookAccessTokenResponseDto.AccessToken))
        {
            throw new Exception("Failed to retrieve Facebook App Access Token.");
        }
        
        string userAccesTokenValidation = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={facebookAccessTokenResponseDto.AccessToken}");

        FacebookUserAccessTokenValidationDto validation =
            JsonSerializer.Deserialize<FacebookUserAccessTokenValidationDto>(userAccesTokenValidation);

        if (validation != null && validation.Data != null && validation.Data.IsValid)
        {
            string userInfoResponse =
                await _httpClient.GetStringAsync(
                    $"http://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}");

            FacebookUserInfoResponseDto facebookUserInfoResponseDto =
                JsonSerializer.Deserialize<FacebookUserInfoResponseDto>(userInfoResponse);

            // UserLoginInfo nesnesi : Dış kaynaktan gelen kullanıcı bilgilerini AspNetUserLogins tablosuna kaydetmemizi sağlar. 
            var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");

            Domain.Entities.Identity.AppUser user =
                await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null; // Kullanıcı Facebook login ile bulunduysa result = true
            if (user == null) // Kullanıcı Facebook login ile bulunamadıysa
            {
                // Email ile ara
                user = await _userManager.FindByEmailAsync(facebookUserInfoResponseDto.Email);
                if (user == null) // Email ile de bulunamadıysa, yeni kullanıcı oluştur
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = facebookUserInfoResponseDto.Email,
                        UserName = facebookUserInfoResponseDto.Email,
                        NameSurname = facebookUserInfoResponseDto.Name,
                    };
                    var identityResult = await _userManager.CreateAsync(user); // AspNetUsers tablosuna kaydettik
                    result = identityResult.Succeeded;
                }
                else
                {
                    // Kullanıcı email ile bulundu
                    // 'result' flag'ini true'ya çekiyoruz ki kod 'if(result)' bloğuna girebilsin.
                    result = true;
                }
            }

            if (result)
            {
                await _userManager.AddLoginAsync(user, info); // AspNetUsersLogins tablosuna da kaydettik

                Token token = _tokenHandler.CreateAccessToken(5);
                return new()
                {
                    Token = token,
                };
            }

        }

        throw new Exception("Invalid External Authentication");

    }
    
}