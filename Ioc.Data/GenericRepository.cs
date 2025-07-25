using Hangfire;
using Ioc.Core;
using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ioc.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : PublicBaseEntity
    {
        private readonly IocDbContext _dbContext;
        private readonly static CacheTech cacheTech = CacheTech.Memory;
        private readonly string cacheKey = $"{typeof(TEntity)}";
        private readonly Func<CacheTech, ICacheService> _cacheService;
        public GenericRepository(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<List<TEntity>> GetPagedData(int pageNumber, int pageSize)
        {
            return await _dbContext.Set<TEntity>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        public async Task<TEntity> GetRnadomAny()
        {
            return await _dbContext.Set<TEntity>().Take(1).AsNoTracking().FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> GetAll(int count)
        {
            return _dbContext.Set<TEntity>().Take(count).AsNoTracking();
        }

        public async Task<List<TEntity>> GetAllAsync(int count)
        {
            return await _dbContext.Set<TEntity>().Take(count).AsNoTracking().ToListAsync();
        }

        public IQueryable<TEntity> GetAllWithInclude(string include)
        {
            return _dbContext.Set<TEntity>().Include(include).AsNoTracking();
        }

        public IQueryable<TEntity> GetAllWithInclude(List<string> includes)
        {
            var firstData = _dbContext.Set<TEntity>();
            foreach (var item in includes)
            {
                firstData.Include(item);
            }
            return firstData.AsNoTracking();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            if (!_cacheService(cacheTech).TryGet(cacheKey, out IReadOnlyList<TEntity> cachedList))
            {
                cachedList = await _dbContext.Set<TEntity>().ToListAsync();
                _cacheService(cacheTech).Set(cacheKey, cachedList);
            }
            return cachedList;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllWithIncludeAsync(string include)
        {
            if (!_cacheService(cacheTech).TryGet(cacheKey, out IReadOnlyList<TEntity> cachedList))
            {
                cachedList = await _dbContext.Set<TEntity>().Include(include).ToListAsync();
                _cacheService(cacheTech).Set(cacheKey, cachedList);
            }
            return cachedList;
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await _dbContext.Set<TEntity>().Where(e => e.ID == id).FirstOrDefaultAsync();
        }

     /*   public async Task<TEntity> GetByUsername(name)
        {
            return await _dbContext.Set<TEntity>().Where(e => e. == id).FirstOrDefaultAsync();

        }*/
        public async Task Create(TEntity entity)
        {
            try
            {
                //entity.SetCreated(GetUserService.GetUserName());
                await _dbContext.Set<TEntity>().AddAsync(entity);
                 _dbContext.ChangeTracker.DetectChanges();
                Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
                await _dbContext.SaveChangesAsync();
                //BackgroundJob.Enqueue(() => RefreshCache());
            }
            catch (Exception ex)
            {
                ex.ManualDBLog(nameof(entity), nameof(entity));
            }
        }

        public async Task CreateInRange(List<TEntity> entity)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entity);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            await _dbContext.SaveChangesAsync();
            //BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task Update(Guid id, TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Set<TEntity>().Update(entity);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var entity = await GetById(id);
                _dbContext.Set<TEntity>().Remove(entity);
                _dbContext.ChangeTracker.DetectChanges();
                Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
                await _dbContext.SaveChangesAsync();
                BackgroundJob.Enqueue(() => RefreshCache());
            }
            catch (Exception ex)
            {
            }
        }

        public async Task RefreshCache()
        {
            _cacheService(cacheTech).Remove(cacheKey);
            var cachedList = await _dbContext.Set<TEntity>().ToListAsync();
            _cacheService(cacheTech).Set(cacheKey, cachedList);
        }

        public bool CheckExist(Guid id)
        {
            return _dbContext.Set<TEntity>().Any(x => x.ID == id);
        }

        public async Task<TEntity> GetByIdWithInclude(Guid id, string include)
        {
            return await _dbContext.Set<TEntity>().Include(include).Where(e => e.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> GetCount()
        {
            return await _dbContext.Set<TEntity>().CountAsync();
        }

        public async Task<string> GetConnectionString()
        {
            return await Task.FromResult(_dbContext.GetConnectionString());
        }

        public async Task<bool> GetDynamicDataExists(string stringFilterName, string descriptionFilter)
        {
            return await (ApplyWhereClause((_dbContext.Set<TEntity>().AsQueryable()), stringFilterName, descriptionFilter)).AnyAsync();
        }

        #region innner method
        private IQueryable<T> ApplyWhereClause<T>(IQueryable<T> query, string propertyName, object value)
        {
            if (string.IsNullOrEmpty(propertyName) || value == null)
            {
                return query;
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(value);
            var equal = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return query.Where(lambda);
        }

        public Task<bool> GetDynamicDataExists(string stringFilterName, string descriptionFilter, Guid? ID)
        {
            return _dbContext.Set<TEntity>().AnyAsync(x=>x.IsDelete);
        }
        #endregion

        /*public async Task<TEntity> GetByIdWithInclude(Guid id)
        {
            return await _dbContext.Set<TEntity>().Where(e => e.ID == id).in.FirstOrDefaultAsync();
        }*/


        /*public async Task<bool> CallSp<T>(out T d, string spName, Dictionary<string, object> Params) where T : class
        {
            string sqlQuery = "EXECUTE dbo."+ spName;

            //SqlParameter p = new SqlParameter("@name", "test");


            List<SqlParameter> prm = new List<SqlParameter>();
            foreach(var obj in Params)
            {
                sqlQuery = sqlQuery+" @" + obj.Key;
                prm.Add(new SqlParameter("@" + obj.Key, obj.Value));

            }

            await Task.Run(() => _dbContext.Set<T>()?
                            .FromSqlRaw<T>(@"exec sp_GetCategoryById @id", prm).ToListAsync());

            *//*new SqlParameter("@variable2", SqlDbType.NVarChar) {Value = ""},
            new SqlParameter("@variable3", SqlDbType.DateTime) {Value = ""},*//*

            var result = _dbContext.Database.ExecuteSqlRaw("Select * from Category", prm);
            *//*var lst = await _dbContext.Database.FromSqlRaw(sqlQuery, p).ToListAsync();
            result = await _dbContext.Database.SqlQuery<result>("EXEC GetProductsList @ProductId",
                                                              new SqlParameter("@ProductId", "1")
                                                              ).ToList();*//*
            return result;
        }*/
    }
}
