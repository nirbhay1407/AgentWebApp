using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ioc.ObjModels
{
    public class PublicBaseModel
    {
        public Guid? ID { get; set; }
        public string? OldValue { get; set; }
        public string SiteCode { get; set; } = "ZZZZ";

    }
}
