using Ioc.Core.DbModel;
using Ioc.Data;

namespace Ioc.Service.Interfaces.Common
{
    public interface ICommonCategory : IGenericRepository<CommonGroup>
    {
        List<CommonGroup> GetCommonGroupByGroup(string GroupDetails);
    }
}
