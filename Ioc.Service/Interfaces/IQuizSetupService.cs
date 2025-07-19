using Ioc.Core.DbModel.Models.Quiz;
using Ioc.Data;

namespace Ioc.Service.Interfaces
{
    public interface IQuizSetupService : IGenericRepository<QuizSetup>
    {
        List<QuizSetup> GetAllWithInc();
        QuizSetup GetAllWithIncById(Guid id);
        List<QuestionSetup> GetAllQuestionSetup(Guid? quesID);
    }
}
