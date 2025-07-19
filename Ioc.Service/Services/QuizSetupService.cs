using Ioc.Core.DbModel.Models.Quiz;
using Ioc.Core.EnumClass;
using Ioc.Data;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Service.Interfaces;
using System.Data.Entity;

namespace Ioc.Service.Services
{
    public class QuizSetupService : GenericRepository<QuizSetup>, IQuizSetupService
    {
        private readonly IocDbContext _dbContext;
        public QuizSetupService(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
            _dbContext = dbContext;
        }

        public List<QuizSetup> GetAllWithInc()
        {
            var data = _dbContext.QuizSetups.Include("QuestionSetup").ToList();
            return data;
        }

        public QuizSetup GetAllWithIncById(Guid id)
        {
            var data = _dbContext.QuizSetups.Where(x => x.ID == id).Include("QuestionSetup").FirstOrDefault();
            return data;
        }
        public List<QuestionSetup> GetAllQuestionSetup(Guid? quesID)
        {
            var data = new List<QuestionSetup>();
            if (quesID == Guid.Empty)
                data = _dbContext.QuestionSetup.ToList();
            else
                data = _dbContext.QuestionSetup.Where(z => z.ID == quesID).ToList();

            data.ForEach(x =>
            {
                x.Answers = _dbContext.Set<AnswerSetup>().Where(y => y.QuestionSetupID == x.ID).AsNoTracking().ToList();
            });
            return data;
        }

        public bool DeleteCashCade(Guid id)
        {
            var data = _dbContext.QuizSetups.Where(x => x.ID == id).FirstOrDefault();
            if (data != null)
            {
                //data.
            }
            return true;
        }
    }
}
