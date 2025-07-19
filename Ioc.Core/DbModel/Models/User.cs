using System.ComponentModel.DataAnnotations.Schema;

namespace Ioc.Core.DbModel.Models
{
    public class User : PublicBaseEntity
    {
        public string? UserName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public int UserProfileId { get; set; }
        [ForeignKey("ID")]
        public virtual UserProfile? UserProfile { get; set; }
    }
}
