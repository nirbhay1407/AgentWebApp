using Ioc.Core.DbModel.SqlLoadModel;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;

namespace Ioc.Service.Services
{
    public class CompleteUserDetailsService : GenericRepository<CompleteUserDet>, ICompleteUserDetailsService
    {
        public CompleteUserDetailsService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
        }
    }
}
