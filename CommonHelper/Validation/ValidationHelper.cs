using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHelper.Validation
{
    public class ValidationHelper
    {
        public static bool TryValidate<T>(T model, out List<ValidationResult> validationResults)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, validationResults, validateAllProperties: true);
        }
    }
}
