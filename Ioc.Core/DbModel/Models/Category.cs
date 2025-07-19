using Microsoft.EntityFrameworkCore;

namespace Ioc.Core.DbModel.Models
{
    public class Category : PublicBaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
