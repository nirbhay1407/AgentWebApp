using Ioc.Core.DbModel;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces.Common;

namespace Ioc.Service.Services.Common
{
    public class SettingService : GenericRepository<Setting>, ISettingService
    {
        private readonly IocDbContext _dbContext;
        public SettingService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
           : base(dbContext, cacheService)
        {
            _dbContext = dbContext;
        }

        public bool CheckExist(string setting)
        {
            return _dbContext!.Setting!.Where(x => x.SettingType == setting).Any();
        }

        public void UpdateDB()
        {
            _dbContext.UpdateDb();
        }
    }
}
