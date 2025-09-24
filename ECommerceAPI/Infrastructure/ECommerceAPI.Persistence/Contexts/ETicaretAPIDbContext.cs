using ECommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Persistence.Contexts;

public class ETicaretAPIDbContext : DbContext
{
    public ETicaretAPIDbContext(DbContextOptions<ETicaretAPIDbContext> options) : base(options)
    {
    }
    // Veri tabanında products tablosu olacak
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    // Bunları oluşturduktan sonra IoC container içine göndermem lazım
    // ServiceRegistration.cs'den yapacağız
}