using Ioc.Core;

namespace Ioc.Data
{
    public interface IGenericRepository<TEntity>
 where TEntity : PublicBaseEntity
    {
        Task<List<TEntity>> GetPagedData(int pageNumber, int pageSize);

        IQueryable<TEntity> GetAll();
        Task<TEntity> GetRnadomAny();
        IQueryable<TEntity> GetAll(int count);
        Task<List<TEntity>> GetAllAsync(int count);
        IQueryable<TEntity> GetAllWithInclude(string include);
        IQueryable<TEntity> GetAllWithInclude(List<string> includes);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetById(Guid id);
        Task<TEntity> GetByIdWithInclude(Guid id, string include);
        //Task<TEntity> GetByIdWithInclude(Guid id);
        bool CheckExist(Guid id);
        Task Create(TEntity entity);
        Task CreateInRange(List<TEntity> entity);
        Task Update(Guid id, TEntity entity);

        Task Delete(Guid id);

        /*Task<dynamic> CallSp(string spName, Dictionary<string, object> Params);*/
        Task<int> GetCount();

        Task<string> GetConnectionString();

        //Task<bool> GetDynamicDataExists(string stringFilterName, string descriptionFilter, Guid? ID = null);
        Task<bool> GetDynamicDataExists(string stringFilterName, string descriptionFilter, Guid? ID);
    }
}
