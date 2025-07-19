using Ioc.Core.DbModel;
using Ioc.Core.EnumClass;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using Ioc.Data;
using Ioc.Service.Interfaces.Validation;
using Microsoft.EntityFrameworkCore;
using Ioc.Core.DbModel.Validation;

namespace Ioc.Service.Services.Validation
{

    public class ValidationRuleRepository : GenericRepository<ValidationRule>, IValidationRuleRepository
    {
        private readonly IocDbContext _context;

        public ValidationRuleRepository(IocDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
            : base(dbContext, cacheService)
        {
            _context = dbContext;
        }

        public async Task<List<ValidationRule>> GetValidationRulesAsync(string modelName)
        {
            var returnVal = new List<ValidationRule>();
            try
            {
                returnVal = await _context.Set<ValidationRule>()
                                 .Where(vr => vr.ModelName == modelName)
                                 .ToListAsync();
            }catch(Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
    }
}



