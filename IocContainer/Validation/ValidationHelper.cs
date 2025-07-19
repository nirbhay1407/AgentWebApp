using Ioc.Service.Interfaces.Validation;

public static class ValidationHelper
{
    public static async Task<(bool IsValid, string ValidationMsg, string WarningMsg)> ValidateAsync<T>(T model, IValidationRuleRepository validationRuleRepository)
    {
        var modelName = typeof(T).Name;
        var validationRules = await validationRuleRepository.GetValidationRulesAsync(modelName);

        var validationErrors = new List<string>();
        var warnings = new List<string>();

        foreach (var rule in validationRules)
        {
            var property = typeof(T).GetProperty(rule.PropertyName);
            if (property == null) continue;

            var value = property.GetValue(model)?.ToString();

          /*  if (rule.RuleType == "Error" && !Regex.IsMatch(value, rule.Rule))
            {
                validationErrors.Add($"{rule.PropertyName} is invalid.");
            }
            else if (rule.RuleType == "Warning" && !Regex.IsMatch(value, rule.Rule))
            {
                warnings.Add($"{rule.PropertyName} might be incorrect.");
            }*/
        }

        var isValid = !validationErrors.Any();
        var validationMsg = string.Join("; ", validationErrors);
        var warningMsg = string.Join("; ", warnings);

        return (isValid, validationMsg, warningMsg);
    }
}
