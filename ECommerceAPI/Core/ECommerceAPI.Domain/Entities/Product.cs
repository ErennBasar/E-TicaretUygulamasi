using ECommerceAPI.Domain.Entities.Common;

namespace ECommerceAPI.Domain.Entities;

public class Product : BaseEntity
{
    public string? Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    
    // Bir product'ın birden çok order'ı olabilir demek
    public ICollection<Order>? Orders { get; set; }
}