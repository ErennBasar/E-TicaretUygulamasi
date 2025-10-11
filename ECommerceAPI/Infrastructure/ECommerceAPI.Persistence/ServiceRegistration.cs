using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Repositories.Customer;
using ECommerceAPI.Application.Repositories.File;
using ECommerceAPI.Application.Repositories.InvoiceFile;
using ECommerceAPI.Application.Repositories.Order;
using ECommerceAPI.Application.Repositories.Product;
using ECommerceAPI.Application.Repositories.ProductImageFile;
using ECommerceAPI.Persistence.Concretes;
using ECommerceAPI.Persistence.Contexts;
using ECommerceAPI.Persistence.Repositories.Customer;
using ECommerceAPI.Persistence.Repositories.File;
using ECommerceAPI.Persistence.Repositories.InvoiceFile;
using ECommerceAPI.Persistence.Repositories.Order;
using ECommerceAPI.Persistence.Repositories.Product;
using ECommerceAPI.Persistence.Repositories.ProductImageFile;
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
        
        // AddDbContext default olarak scoped olarak eklediği için Repository servislerini de scoped olarak ekledik.
        
        // Sigleton'da hepsi için bir nesne oluşturulur. 
        // Scoped'da her request için oluşturulan nesne iş bittikten sonra imha (dispose) edilir.
        services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
        services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
        services.AddScoped<IOrderReadRepository, OrderReadRepository>();
        services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
        services.AddScoped<IFileReadRepository, FileReadRepository>();
        services.AddScoped<IFileWriteRepository, FileWriteRepository>();
        services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
        services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();
        services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
        services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();
        
    } 
    
}