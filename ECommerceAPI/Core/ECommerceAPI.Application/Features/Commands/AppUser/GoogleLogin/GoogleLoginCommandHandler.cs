using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ECommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    private readonly ITokenHandler  _tokenHandler;
    private readonly IConfiguration _configuration;

    public GoogleLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _configuration = configuration;
    }

    public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
    {
        var googleClientId = _configuration["Google:ClientId"];
        
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string> {googleClientId}
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

        // UserLoginInfo nesnesi : Dış kaynaktan gelen kullanıcı bilgilerini AspNetUserLogins tablosuna kaydetmemizi sağlar. 
        var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);

        Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        
        bool result = user != null;
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = payload.Email,
                    UserName = payload.Email,
                    NameSurname = payload.Name,
                };
                var identityResult = await _userManager.CreateAsync(user); // AspNetUsers tablosuna kaydettik
                result = identityResult.Succeeded;
            }
        }

        if (result)
            await _userManager.AddLoginAsync(user, info); // AspNetUsersLogins tablosuna da kaydettik
        else
            throw new Exception("Invalid External Authentication");

        Token token = _tokenHandler.CreateAccessToken(5);
        
        return new()
        {
            Token = token
        };
    }
}