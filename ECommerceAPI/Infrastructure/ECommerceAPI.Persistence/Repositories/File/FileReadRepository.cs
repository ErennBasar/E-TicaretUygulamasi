using ECommerceAPI.Application.Repositories.File;
using ECommerceAPI.Persistence.Contexts;

namespace ECommerceAPI.Persistence.Repositories.File;

public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
{
    public FileReadRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}