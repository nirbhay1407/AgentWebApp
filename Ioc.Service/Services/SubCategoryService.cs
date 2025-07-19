using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;

namespace Ioc.Service.Services
{
    public class SubCategoryService : GenericRepository<SubCategory>, ISubCategoryService
    {
        public SubCategoryService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
        }

        /*public async Task getData()
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("Id", "137B232D-7A17-45BE-12FB-08DB3DA340AB");
            var result = await CallSp("sp_GetCategory", keyValuePairs);
        }*/
    }
}
