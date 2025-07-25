using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;

namespace Ioc.Service.Services
{
    public class UserService : GenericRepository<User>, IUserService
    {
        public UserService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
            : base(dbContext, cacheService)
        {

        }


        public IQueryable<User> GetUsers()
        {
            return GetAll();
        }

        public async Task<User> GetUser(Guid id)
        {
            return await GetById(id);
        }

       /* public async Task<User> GetUser(string name)
        {
            return await GetByUsername(name);
        }*/

        public async Task InsertUser(User user)
        {
            await Create(user);
        }

        public async Task UpdateUser(User user)
        {
            await Update(user.ID, user);
        }

        public async Task DeleteUser(Guid id)
        {
            //await this.Delete(user.UserProfile.ID);
            await this.Delete(id);
        }
    }
}
