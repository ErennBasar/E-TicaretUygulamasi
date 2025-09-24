using ECommerceAPI.Domain.Entities.Common;

namespace ECommerceAPI.Domain.Entities;

public class Customer : BaseEntity
{
    public string? Name { get; set; }
    
    //Bir customer'ın birden fazla order'ı olabilir
    public ICollection<Order>? Orders { get; set; }
}