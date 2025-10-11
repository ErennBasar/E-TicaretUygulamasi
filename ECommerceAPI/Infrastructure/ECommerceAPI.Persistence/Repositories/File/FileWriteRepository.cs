using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.File;
using ECommerceAPI.Persistence.Contexts;

namespace ECommerceAPI.Persistence.Repositories.File;

public class FileWriteRepository : WriteRepository<Domain.Entities.File>, IFileWriteRepository
{
    public FileWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}