using Microsoft.Extensions.Configuration;

namespace ECommerceAPI.Persistence;

public class Configuration
{
    public static string? ConnectionString
    {
        get
        {
            ConfigurationManager configurationManager = new();
            
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ECommerceAPI.API/"));
            configurationManager.AddJsonFile("appsettings.json");
            
            return configurationManager.GetConnectionString("ETicaretAPIDb");
        }
    }
}