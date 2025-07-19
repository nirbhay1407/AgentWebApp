using Ioc.Core.DbModel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ioc.ObjModels.Model
{
    public class UserModelDt : PublicBaseModel
    {
        public string? UserName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? CreatedAt { get; set; }

        public int UserProfileId { get; set; }
        [ForeignKey("ID")]
        public virtual UserProfileModel? UserProfile { get; set; }
    }
}
