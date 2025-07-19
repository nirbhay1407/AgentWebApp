using System.ComponentModel.DataAnnotations.Schema;

namespace Ioc.Core.DbModel.Models
{
    public partial class SubCategory : PublicBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}
