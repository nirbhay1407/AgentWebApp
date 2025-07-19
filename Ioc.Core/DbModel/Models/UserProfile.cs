namespace Ioc.Core.DbModel.Models
{
    public class UserProfile : PublicBaseEntity
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
    }
}
