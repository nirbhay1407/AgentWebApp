namespace Ioc.Core.DbModel
{
    public class UserClaim : PublicBaseEntity
    {
        public string ClaimName { get; set; } = string.Empty;
        public bool ClaimValue { get; set; }
    }
}
