namespace Ioc.Core.DbModel.Models
{

    public class SalesPerson : PublicBaseEntity
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int EmployeeId { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public Address? Address { get; set; }

        public SalesPerson() { }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, Employee ID: {EmployeeId}, Email: {Email}, Phone: {PhoneNumber}, Address: {Address}";
        }
    }
}
