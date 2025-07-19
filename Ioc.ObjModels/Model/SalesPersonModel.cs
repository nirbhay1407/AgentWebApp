using Ioc.ObjModels.Model.CommonModel;
using System.ComponentModel.DataAnnotations;

namespace Ioc.ObjModels.Model
{

    public class SalesPersonModel : PublicBaseModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public AddressModel Address { get; set; }

        public SalesPersonModel() { }

        public SalesPersonModel(string firstName, string lastName, int employeeId, string email, string phoneNumber, AddressModel address)
        {
            FirstName = firstName;
            LastName = lastName;
            EmployeeId = employeeId;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, Employee ID: {EmployeeId}, Email: {Email}, Phone: {PhoneNumber}, Address: {Address}";
        }
    }
}
