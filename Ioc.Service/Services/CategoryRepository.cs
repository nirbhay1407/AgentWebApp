using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ioc.Service.Services
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public IocDbContext _dbContext;
        public CategoryRepository(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
            : base(dbContext, cacheService)
        {
            _dbContext = dbContext;
        }

        public async Task<Category> GetDataBySp(Guid id)
        {
            /*List<SqlParameter> prm = new List<SqlParameter>();

            foreach (var obj in Params)
            {*/
            //sqlQuery = sqlQuery + " @" + obj.Key;
            SqlParameter prm = new SqlParameter("@id", id);

            //}
            //var aa =  await Task.Run(() => _dbContext.Set<Category>()?
            // .FromSqlRaw<Category>(@"exec sp_GetCategoryById @id", prm).ToListAsync());
            var aa = await _dbContext.Set<Category>()?
             .FromSqlRaw<Category>(@"exec sp_GetCategoryById @id", prm).ToListAsync();
            _dbContext.Database.ExecuteSqlRawAsync(@"exec sp_GetCategoryById @id", prm);
            return aa.FirstOrDefault();
        }

        public async Task<Category> GetCoolestCategory()
        {
            return await GetAll()
                .OrderByDescending(c => c.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CheckExistByName(string name)
        {
            return await _dbContext.Set<Category>().AnyAsync(x => x.Name == name);
        }
    }
}
