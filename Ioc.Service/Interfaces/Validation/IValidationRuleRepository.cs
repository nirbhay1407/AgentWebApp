
using Ioc.Core.DbModel.Validation;

namespace Ioc.Service.Interfaces.Validation
{
    public interface IValidationRuleRepository
    {
        Task<List<ValidationRule>> GetValidationRulesAsync(string modelName);
    }
}
