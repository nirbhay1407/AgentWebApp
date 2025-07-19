using Ioc.Core.DbModel;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces.Common;

namespace Ioc.Service.Services.Common
{
    public class CommonCategory : GenericRepository<CommonGroup>, ICommonCategory
    {
        public CommonCategory(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
            : base(dbContext, cacheService)
        {

        }

        public List<CommonGroup> GetCommonGroupByGroup(string GroupDetails)
        {
            return GetAll().Where(x => x.GroupDetails == GroupDetails).ToList();
        }
    }
}
