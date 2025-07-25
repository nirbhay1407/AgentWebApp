using Ioc.Core.DbModel;
using Ioc.Core.DbModel.Models;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Ioc.Service.Services.Common
{
    public class SettingService : ISettingService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly object _lock = new();
        private readonly Dictionary<string, string> _settings = new();
        private readonly static CacheTech cacheTech = CacheTech.Memory;
        private readonly Func<CacheTech, ICacheService> _cacheService;

        public SettingService(IServiceProvider serviceProvider, Func<CacheTech, ICacheService> cacheService)
        {
            _serviceProvider = serviceProvider;
            _cacheService = cacheService;
            LoadSettings();

        }

    /*    public bool CheckExist(string setting)
        {
            return _dbContext!.Setting!.Where(x => x.SettingType == setting).Any();
        }
*/

        private void LoadSettings()
        {
            lock (_lock)
            {
                _settings.Clear();
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IocDbContext>();
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _cacheService(cacheTech).Set("SiteCode", "NBVC");

                List<Setting> settings = dbContext.Setting.ToList();
                foreach (var setting in settings)
                    _cacheService(cacheTech).Set(setting.DisplayName, setting.SettingType);
            }
        }

        public string Get(string key)
        {
            if (_settings.TryGetValue(key, out var value))
                return value;
            return null;
        }

        public void Refresh() => LoadSettings();

        //_cacheService(cacheTech).Set(cacheKey, cachedList);

    }
}
