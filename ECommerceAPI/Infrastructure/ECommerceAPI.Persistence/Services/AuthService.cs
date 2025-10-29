using System.Text.Json;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.Facebook;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using ECommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ECommerceAPI.Persistence.Services;

public class AuthService : IAuthService
{
    readonly HttpClient _httpClient;
    readonly IConfiguration _configuration;
    readonly UserManager<AppUser>  _userManager;
    readonly ITokenHandler  _tokenHandler;
    readonly SignInManager<AppUser> _signInManager;

    public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
        _httpClient = httpClientFactory.CreateClient();
    }

    async Task<Token> CreateUserExternalAsync(AppUser? user, string email, string name, UserLoginInfo info, int accessTokenLifeTimeSeconds)
    {
        bool result = user != null; // Kullanıcı Facebook login ile bulunduysa result = true
        if (user == null) // Kullanıcı Facebook login ile bulunamadıysa
        {
            // Email ile ara
            user = await _userManager.FindByEmailAsync(email);
            if (user == null) // Email ile de bulunamadıysa, yeni kullanıcı oluştur
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    UserName = email,
                    NameSurname = name,
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
            // Kullanıcı sisteme daha önce email ile kayıt olduysa veya yeni oluşturulduysa
            // Harici login bilgisini AspNetUsersLogins tablosuna ekle
            IdentityResult addLoginResult = await _userManager.AddLoginAsync(user!, info); 
            
            // AddLoginAsync başarısız olursa (örneğin kullanıcı zaten bu login'e sahipse, ki FindByLoginAsync bunu engeller, ama bir kontrol eklemek iyi olur)
            if (!addLoginResult.Succeeded && addLoginResult.Errors.All(e => e.Code != "LoginAlreadyAssociated"))
            {
                // Kritik hata, loglanmalı
                throw new Exception($"Failed to associate external login: {string.Join(", ", addLoginResult.Errors.Select(e => e.Description))}");
            }

            // Token oluştur
            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTimeSeconds);
            return token;
            
            // await _userManager.AddLoginAsync(user, info); // AspNetUsersLogins tablosuna da kaydettik
            //
            // Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTimeSeconds);
            //     
            // return token;

        }
        throw new Exception("Invalid External Authentication or User Creation Failed");
    }

    public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTimeSeconds)
    {
        try
        {
            string accessTokenResponse =
                await _httpClient.GetStringAsync
                    ($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["Facebook:ClientId"]}&client_secret={_configuration["Facebook:ClientSecret"]}&grant_type=client_credentials");

            FacebookAccessTokenResponseDto? facebookAccessTokenResponseDto =
                JsonSerializer.Deserialize<FacebookAccessTokenResponseDto>(accessTokenResponse);

            // Hata kontrolü: Token alınamadıysa
            if (facebookAccessTokenResponseDto == null ||
                string.IsNullOrEmpty(facebookAccessTokenResponseDto.AccessToken))
            {
                throw new Exception(
                    "Failed to retrieve Facebook App Access Token. Response was incomplete or invalid.");
            }

            string userAccesTokenValidation = await _httpClient.GetStringAsync(
                $"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponseDto.AccessToken}");

            FacebookUserAccessTokenValidationDto? validation =
                JsonSerializer.Deserialize<FacebookUserAccessTokenValidationDto>(userAccesTokenValidation);

            if (validation != null && validation?.Data != null && validation.Data.IsValid)
            {
                string userInfoResponse =
                    await _httpClient.GetStringAsync(
                        $"http://graph.facebook.com/me?fields=email,name&access_token={authToken}");

                FacebookUserInfoResponseDto? facebookUserInfoResponseDto =
                    JsonSerializer.Deserialize<FacebookUserInfoResponseDto>(userInfoResponse);

                if (facebookUserInfoResponseDto == null || string.IsNullOrEmpty(facebookUserInfoResponseDto.Email))
                {
                    // Facebook'tan e-posta gelmediyse, bu bir izin eksikliği veya hata sinyali olabilir.
                    throw new Exception("Facebook did not return email information. Permission issue?");
                }

                // UserLoginInfo nesnesi : Dış kaynaktan gelen kullanıcı bilgilerini AspNetUserLogins tablosuna kaydetmemizi sağlar. 
                var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");

                AppUser? user =
                    await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                return await CreateUserExternalAsync(user, facebookUserInfoResponseDto.Email,
                    facebookUserInfoResponseDto.Name, info, accessTokenLifeTimeSeconds);
            }
        }
        catch (HttpRequestException ex)
        {
            // HTTP çağrılarında (Facebook API) bir hata oluşursa yakala
            throw new Exception($"Facebook API connection error: {ex.Message}");
        }
        catch (System.Text.Json.JsonException ex)
        {
            // JSON serileştirme/deserileştirme hatası
            throw new Exception($"Facebook JSON deserialization error: {ex.Message}");
        }
        catch (Exception ex) when (ex.Message.Contains("Facebook did not return email information") || ex.Message.Contains("Failed to retrieve Facebook App Access Token"))
        {
            // Kendi attığımız spesifik hataları tekrar fırlat
            throw;
        }
        catch (Exception)
        {
            // Diğer tüm hataları yakala ve genel bir kimlik doğrulama hatası fırlat
            throw new AuthenticationErrorException(); 
        }

        throw new Exception("Invalid External Authentication");
    }

    public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTimeSeconds)
    {
        try
        {

            var googleClientId = _configuration["Google:ClientId"];
            if (string.IsNullOrEmpty(googleClientId))
            {
                throw new Exception("Google ClientId configuration is missing or empty.");
            }
            
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string?> {googleClientId}
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            
            if (string.IsNullOrEmpty(payload.Email))
            {
                // Google'dan e-posta gelmediyse (ki bu nadirdir), yine de kontrol edelim.
                throw new Exception("Google did not return email information.");
            }

            // UserLoginInfo nesnesi : Dış kaynaktan gelen kullanıcı bilgilerini AspNetUserLogins tablosuna kaydetmemizi sağlar. 
            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");

            AppUser? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifeTimeSeconds);
        }
        catch (InvalidJwtException ex)
        {
            // JWT token doğrulama hatası (Token süresi dolmuş, imzası yanlış vb.)
            throw new Exception($"Google Token Validation Failed: {ex.Message}");
        }
        catch (Exception ex) when (ex.Message.Contains("Google ClientId configuration is missing"))
        {
            // Kendi attığımız config hatasını tekrar fırlat
            throw;
        }
        catch (Exception)
        {
            // Diğer tüm hataları yakala ve genel bir kimlik doğrulama hatası fırlat
            throw new AuthenticationErrorException(); 
        }
    }
    
    public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTimeSeconds)
    {
        // Giriş yaparken Username kullandıysa
        AppUser? user = await _userManager.FindByNameAsync(usernameOrEmail);
        // Giriş yaparken Email kullandıysa
        if(user == null)
            user = await _userManager.FindByEmailAsync(usernameOrEmail);
        
        if(user == null)
            throw new UserNotFoundException();
        
        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (result.Succeeded)
        {
            // Yetkiler belirlenecek
            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTimeSeconds);
            
            return token;
        }

        // return new LoginUserErrorCommandResponse()
        // {
        //     Message = "Invalid username or password !!! "
        // };

        throw new AuthenticationErrorException();
    }
}