using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces.FakeDataHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ioc.Service.Services.FakeDataHelp
{
    public class RandomGenHelpService : GenericRepository<RandomGenHelp>, IRandomGenHelpService
    {
        public IocDbContext _dbContext;
        public RandomGenHelpService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
            : base(dbContext, cacheService)
        {
            _dbContext = dbContext;
        }
    }
}
