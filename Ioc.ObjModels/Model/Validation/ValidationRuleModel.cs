using Ioc.Core;
using Ioc.ObjModels.Model.CommonModel;
using System.ComponentModel.DataAnnotations;

namespace Ioc.ObjModels.Model.Validation
{
    public class ValidationRuleModel : PublicBaseModel
    {
        public string? ModelName { get; set; }
        public string? PropertyName { get; set; }
        public string? RuleType { get; set; } // 'Error' or 'Warning'
        public string? Rule { get; set; }
    }
}
