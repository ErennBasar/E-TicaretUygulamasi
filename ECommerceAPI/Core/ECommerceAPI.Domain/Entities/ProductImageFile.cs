namespace ECommerceAPI.Domain.Entities;

public class ProductImageFile : File
{
    // Bir resim sadece bir ürüne ait olabilir
    public ICollection<Product>? Products { get; set; }
}