using ECommerceAPI.Application.Repositories.ProductImageFile;
using ECommerceAPI.Persistence.Contexts;

namespace ECommerceAPI.Persistence.Repositories.ProductImageFile;

public class ProductImageFileReadRepository : ReadRepository<Domain.Entities.ProductImageFile>, IProductImageFileReadRepository
{
    public ProductImageFileReadRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}