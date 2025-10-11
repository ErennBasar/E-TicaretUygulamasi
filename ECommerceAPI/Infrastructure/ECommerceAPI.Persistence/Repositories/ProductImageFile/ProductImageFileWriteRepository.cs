using ECommerceAPI.Application.Repositories.ProductImageFile;
using ECommerceAPI.Persistence.Contexts;

namespace ECommerceAPI.Persistence.Repositories.ProductImageFile;

public class ProductImageFileWriteRepository : WriteRepository<Domain.Entities.ProductImageFile>, IProductImageFileWriteRepository
{
    public ProductImageFileWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}