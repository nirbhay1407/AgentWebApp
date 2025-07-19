using Ioc.Core.DbModel.Models.Quiz;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;

namespace Ioc.Service.Services
{
    public class AnswerSetupService : GenericRepository<AnswerSetup>, IAnswerSetupService
    {
        private readonly IocDbContext _answerSetup;
        public AnswerSetupService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
        }

        public bool CheckByQuesId(Guid iD)
        {
            return _answerSetup.AnswerSetup.Where(x => x.QuestionSetupID == iD).Any();
        }

        public List<AnswerSetup> GetByQuesId(Guid iD)
        {
            return _answerSetup.AnswerSetup.Where(x => x.QuestionSetupID == iD).ToList();
        }
    }
}
