namespace Ioc.Core.DbModel.Models
{
    public class Customer : PublicBaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Contact { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public Address? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
