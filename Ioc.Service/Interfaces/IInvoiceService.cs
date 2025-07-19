using Ioc.Core.DbModel.Models;
using Ioc.Data;

namespace Ioc.Service.Interfaces
{
    public interface IInvoiceService : IGenericRepository<InvoiceNew>
    {
        Task<InvoiceNew> GetWithAllOfData(Guid id);
        Task<List<InvoiceNew>> GetAllInc();
    }
}
