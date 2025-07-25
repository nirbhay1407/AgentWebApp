using Ioc.Core.DbModel.Models.SiteInfo;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces.Common;

namespace Ioc.Service.Services.Common
{
    public class SiteSetupService : GenericRepository<SiteSetup>,  ISiteSetupService
    {
        public SiteSetupService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
        }
    }
}
