using Ioc.Core.DbModel.Models.SiteInfo;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;

namespace Ioc.Service.Services
{
    public class CompanyService : GenericRepository<Company>, ICompanyService
    {
        public CompanyService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
        }
    }
}
