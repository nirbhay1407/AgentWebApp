using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ioc.Core
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        [NotMapped]
        public string IdentityRole { get; set; }
        [NotMapped]
        public string UserClaim { get; set; }
    }
}
