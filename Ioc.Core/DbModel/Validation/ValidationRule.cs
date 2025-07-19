namespace Ioc.Core.DbModel.Validation
{
    public class ValidationRule : PublicBaseEntity
    {
        public string? ModelName { get; set; }
        public string? PropertyName { get; set; }
        public string? RuleType { get; set; } // 'Error' or 'Warning'
        public string? Rule { get; set; }
    }
}
