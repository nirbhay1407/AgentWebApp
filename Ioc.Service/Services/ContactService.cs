using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;

namespace Ioc.Service.Services
{
    public class ContactService : GenericRepository<Contact>, IContactService
    {
        public ContactService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
        }
    }
}
