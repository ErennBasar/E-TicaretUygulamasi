using ECommerceAPI.Application.Repositories.InvoiceFile;
using ECommerceAPI.Persistence.Contexts;

namespace ECommerceAPI.Persistence.Repositories.InvoiceFile;

public class InvoiceFileWriteRepository : WriteRepository<Domain.Entities.InvoiceFile>, IInvoiceFileWriteRepository
{
    public InvoiceFileWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}