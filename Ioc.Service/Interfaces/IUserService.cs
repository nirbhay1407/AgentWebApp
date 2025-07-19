using Ioc.Core.DbModel.Models;
using Ioc.Data;

namespace Ioc.Service.Interfaces
{
    public interface IUserService : IGenericRepository<User>
    {
        IQueryable<User> GetUsers();
        Task<User> GetUser(Guid id);
        Task InsertUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(Guid id);
    }
}
