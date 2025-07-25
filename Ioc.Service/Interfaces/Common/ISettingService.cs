using Ioc.Core.DbModel;
using Ioc.Data;

namespace Ioc.Service.Interfaces.Common
{
    public interface ISettingService 
    {
        /*bool CheckExist(string setting);
        void UpdateDB();*/
        string Get(string key);
    }
}
