using Ioc.Core.DbModel.Models;
using Ioc.Data;

namespace Ioc.Service.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetCoolestCategory();
        Task<Category> GetDataBySp(Guid id);
        Task<bool> CheckExistByName(String name);
    }
}
