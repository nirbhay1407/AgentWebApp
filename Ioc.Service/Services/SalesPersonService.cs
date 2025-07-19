using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;

namespace Ioc.Service.Services
{
    public class SalesPersonService : GenericRepository<SalesPerson>, ISalesPersonService
    {
        public SalesPersonService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
        }
    }
}
