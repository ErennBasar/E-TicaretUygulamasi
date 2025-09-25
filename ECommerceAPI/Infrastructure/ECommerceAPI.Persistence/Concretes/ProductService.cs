using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Persistence.Concretes;

public class ProductService : IProductService
{
    public List<Product> GetProducts()
        => new() //target type: dönüş tipi ne ise (List<Product>) direkt ona dönüştürür
        {
            new ()
            {
                Id = Guid.NewGuid(),
                Name = "Product 1",
                Stock = 10,
                Price = 100
            },
            new ()
            {
                Id = Guid.NewGuid(),
                Name = "Product 2",
                Stock = 20,
                Price = 200
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product 3",
                Stock = 30,
                Price = 300
            }
        };
}