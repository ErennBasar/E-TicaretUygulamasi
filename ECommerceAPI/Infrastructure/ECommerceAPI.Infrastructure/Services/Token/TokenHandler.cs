using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ECommerceAPI.Application.Abstractions.Token;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceAPI.Infrastructure.Services.Token;

public class TokenHandler : ITokenHandler
{
    readonly IConfiguration _configuration;

    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Application.DTOs.Token CreateAccessToken(int minutesToExpire)
    {
        Application.DTOs.Token token = new();

        // Security Key'in simetriğini alıyoruz
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
        
        // Şifrelenmiş kimliği oluşturuyoruz
        SigningCredentials signingCredentials = new(securityKey,SecurityAlgorithms.HmacSha256);
        
        // Oluşturulacak token ayarlarını veriyoruz
        token.Expiration = DateTime.UtcNow.AddMinutes(minutesToExpire);

        JwtSecurityToken securityToken = new(
            audience: _configuration["Token:Audience"],
            issuer: _configuration["Token:Issuer"],
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials
            );
        
        // Token oluşturucu sınıfından bir örnek alıyoruz
        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(securityToken);
        
        return token;
    }
    
}