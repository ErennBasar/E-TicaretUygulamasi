using Microsoft.AspNetCore.Identity; // IdentityUser

namespace ECommerceAPI.Domain.Entities.Identity;

public class AppUser : IdentityUser<string> // Identity User içinde verilerimizi tutabileceğimiz property'ler var yoksa burda oluşturuyoruz
{
    public string NameSurname { get; set; }
}