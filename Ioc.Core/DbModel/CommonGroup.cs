using System.ComponentModel.DataAnnotations.Schema;

namespace Ioc.Core.DbModel
{
    public class CommonGroup : PublicBaseEntity
    {
        public string GroupDetails { get; set; }
        public string? value { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
    }
}
