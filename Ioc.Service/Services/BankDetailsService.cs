using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;

namespace Ioc.Service.Services
{
    public class BankDetailsService : GenericRepository<BankDetails>, IBankDetailsService
    {
        public IocDbContext _dbContext;
        public BankDetailsService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
            : base(dbContext, cacheService)
        {
            _dbContext = dbContext;
        }
    }
}
