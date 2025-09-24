using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Persistence.Concretes;
using ECommerceAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceAPI.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services) //Microsoft.Extensions.DependencyInjection.Abstractions
    {
        // IoC container içinde artık ama Program.cs içinden AddPersistenceServices ile çağrılması lazım
        services.AddSingleton<IProductService, ProductService>();

        // Postgresql kullanacağım için Nuget'den Npgsql.EntityFrameworkCore.PostgreSQL ekledim
        services.AddDbContext<ETicaretAPIDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
    } 
    
}