using Hangfire;
using Ioc.Core;
using Ioc.Core.EnumClass;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ioc.Data.UnitOfWorkRepo
{
    public class UnitOfWork : IUnitOfWork       
    {
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private readonly IocDbContext _dbContext;
        private readonly static CacheTech cacheTech = CacheTech.Memory;
        private readonly Func<CacheTech, ICacheService> _cacheService;

        public UnitOfWork(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : PublicBaseEntity
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IGenericRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new GenericRepository<TEntity>(_dbContext, _cacheService);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}
