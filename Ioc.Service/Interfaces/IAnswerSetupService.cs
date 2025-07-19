using Ioc.Core.DbModel.Models.Quiz;
using Ioc.Data;

namespace Ioc.Service.Interfaces
{
    public interface IAnswerSetupService : IGenericRepository<AnswerSetup>
    {
        bool CheckByQuesId(Guid iD);

        List<AnswerSetup> GetByQuesId(Guid iD);
    }
}
