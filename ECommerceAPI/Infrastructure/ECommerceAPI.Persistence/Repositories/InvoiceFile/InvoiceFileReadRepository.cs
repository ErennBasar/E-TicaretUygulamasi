using ECommerceAPI.Application.Repositories.InvoiceFile;
using ECommerceAPI.Persistence.Contexts;

namespace ECommerceAPI.Persistence.Repositories.InvoiceFile;

public class InvoiceFileReadRepository : ReadRepository<Domain.Entities.InvoiceFile>, IInvoiceFileReadRepository
{
    public InvoiceFileReadRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}