using Ioc.Core.DbModel;
using Ioc.Data;

namespace Ioc.Service.Interfaces.Common
{
    public interface ISettingService : IGenericRepository<Setting>
    {
        bool CheckExist(string setting);
        void UpdateDB();
    }
}
