using Ioc.Core.DbModel;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces.Common;

namespace Ioc.Service.Services.Common
{
    public class AddressService : GenericRepository<Address>, IAddressService
    {
        public AddressService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
            : base(dbContext, cacheService)
        {

        }
    }
}
