using ECommerceAPI.Domain.Entities.Common;

namespace ECommerceAPI.Domain.Entities;

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    
    // Bir order'ın birden fazla product'ı olduğunu ifade ediyor
    public ICollection<Product>? Products { get; set; }
    
    // Siparişin bir Customer'ı olabilir
    public Customer? Customer { get; set; }
}