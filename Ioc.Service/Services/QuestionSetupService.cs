using Ioc.Core.DbModel.Models.Quiz;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;

namespace Ioc.Service.Services
{
    public class QuestionSetupService : GenericRepository<QuestionSetup>, IQuestionSetupService
    {
        public QuestionSetupService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
        }
    }
}
