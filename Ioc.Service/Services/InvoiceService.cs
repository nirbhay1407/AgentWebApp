using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ioc.Service.Services
{
    public class InvoiceService : GenericRepository<InvoiceNew>, IInvoiceService
    {
        public IocDbContext _dbContext;
        public InvoiceService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
            : base(dbContext, cacheService)
        {
            _dbContext = dbContext;
        }
        public async Task<List<InvoiceNew>> GetAllInc()
        {
            var parameters = new SqlParameter[] { };
            var a = await _dbContext.ExecuteScalarAsync("Select TOP 1 Username from [dbo].[AspNetUsers]", System.Data.CommandType.Text, parameters);
            return await _dbContext.InvoiceNew.Include("Customer.Address").Include("Company.CompanyAddress").ToListAsync();
        }

        public async Task<InvoiceNew> GetWithAllOfData(Guid id)
        {
            return await _dbContext.InvoiceNew.Where(x => x.ID == id).Include("Customer.Address").Include("Company.ContactDetails").Include("Company.BankDetails").FirstOrDefaultAsync();
        }
    }
}
